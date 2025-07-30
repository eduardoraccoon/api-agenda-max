using api_iso_med_pg.Utilities;
using api_iso_med_pg.DTOs.Equipamiento;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EquipamientosController : ControllerBase
    {
        private readonly IEquipamientoRepository _repository;
        private readonly IMapper _mapper;
        public EquipamientosController(IEquipamientoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<EquipamientoDto>>>> Get()
        {
            try
            {
                var equipamientos = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EquipamientoDto>>(equipamientos);
                var reply = dtos != null && dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<EquipamientoDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<EquipamientoDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<EquipamientoDto>>> Get(int id)
        {
            try
            {
                var equipamiento = await _repository.GetByIdAsync(id);
                if (equipamiento == null)
                    return NotFound(new BaseResponse<EquipamientoDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<EquipamientoDto>(equipamiento);
                return Ok(new BaseResponse<EquipamientoDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<EquipamientoDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post([FromForm] CreateEquipamientoDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                     dto.CreatedBy = userId;
                }
                var equipamiento = _mapper.Map<Equipamiento>(dto);

                // Manejar la subida de imagen si existe
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
                    var filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/Equipamientos/");
                    if (!Directory.Exists(filesPath))
                        Directory.CreateDirectory(filesPath);
                    var fileExtension = Path.GetExtension(dto.ArchivoImagen.FileName);
                    var fileName = $"equipamiento_{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(filesPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ArchivoImagen.CopyToAsync(stream);
                    }
                    equipamiento.UrlImagen = $"/Files/Equipamientos/{fileName}";
                }

                await _repository.AddAsync(equipamiento);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_QUERY,
                    Message = ReplyMessage.MESSAGE_QUERY
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
        public async Task<IActionResult> Put([FromForm] UpdateEquipamientoDto dto)
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
                    // Asignar usuario actualizador
                }
                _mapper.Map(dto, existing);

                // Manejar la subida de imagen si existe
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
                    var filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/Equipamientos/");
                    if (!Directory.Exists(filesPath))
                        Directory.CreateDirectory(filesPath);
                    var fileExtension = Path.GetExtension(dto.ArchivoImagen.FileName);
                    var fileName = $"equipamiento_{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(filesPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ArchivoImagen.CopyToAsync(stream);
                    }
                    existing.UrlImagen = $"/Files/Equipamientos/{fileName}";
                }

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


        [HttpGet("generate-qr/{id}")]
        public IActionResult GenerateQRCode(int id)
        {
            string qrText = $"https://iso-med.acerosur.com/equipamientos/qr/{id}";
            var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrData);
            byte[] qrBytes = qrCode.GetGraphic(20);
            return File(qrBytes, "image/png");
        }
    }
}
