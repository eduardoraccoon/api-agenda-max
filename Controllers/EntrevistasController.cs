using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.DTOs;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Utilities;
using AutoMapper;

namespace api_iso_med_pg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntrevistasController : ControllerBase
    {
        private readonly IEntrevistaRepository _repository;
        private readonly IMapper _mapper;

        public EntrevistasController(IEntrevistaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<EntrevistaDto>>>> Get()
        {
            try
            {
                var entrevistas = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EntrevistaDto>>(entrevistas);
                var reply = dtos != null && dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<EntrevistaDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<EntrevistaDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<EntrevistaDto>>> GetById(int id)
        {
            try
            {
                var entrevista = await _repository.GetByIdAsync(id);
                if (entrevista == null)
                    return NotFound(new BaseResponse<EntrevistaDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<EntrevistaDto>(entrevista);
                return Ok(new BaseResponse<EntrevistaDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<EntrevistaDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post([FromForm] CreateEntrevistaDto dto)
        {
            try
            {
                dto.CreatedBy = 0;
                dto.CreatedAt = DateTime.UtcNow;
                var entrevista = _mapper.Map<Entrevista>(dto);
                if (dto.ArchivoImagen != null && dto.ArchivoImagen.Length > 0)
                {
                    var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                    if (!allowedTypes.Contains(dto.ArchivoImagen.ContentType.ToLower()))
                    {
                        return BadRequest(new BaseResponse<string>
                        {
                            IsSuccess = false,
                            Message = "Solo se permiten archivos de imagen (JPEG, PNG, GIF)"
                        });
                    }
                    if (dto.ArchivoImagen.Length > 5 * 1024 * 1024)
                    {
                        return BadRequest(new BaseResponse<string>
                        {
                            IsSuccess = false,
                            Message = "El archivo no puede ser mayor a 5MB"
                        });
                    }
                    var filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/Entrevistas/");
                    if (!Directory.Exists(filesPath))
                        Directory.CreateDirectory(filesPath);
                    var fileExtension = Path.GetExtension(dto.ArchivoImagen.FileName);
                    var fileName = $"entrevista_{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(filesPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ArchivoImagen.CopyToAsync(stream);
                    }
                    entrevista.UrlImagen = $"/Files/Entrevistas/{fileName}";
                }
                await _repository.AddAsync(entrevista);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_SAVE,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateEntrevistaDto dto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing == null)
                {
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                }

                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.UpdatedBy = userId;
                }
                dto.UpdatedAt = DateTime.UtcNow;
                _mapper.Map(dto, existing);

                await _repository.UpdateAsync(existing);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_UPDATE,
                    Message = ReplyMessage.MESSAGE_UPDATE
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });

                var userIdClaim = User.FindFirst("id")?.Value;
                int deletedBy = 0;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    deletedBy = userId;
                }
                await _repository.DeleteAsync(id, deletedBy);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_DELETE,
                    Message = ReplyMessage.MESSAGE_DELETE
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }
    }
}