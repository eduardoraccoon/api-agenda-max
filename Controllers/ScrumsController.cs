using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.DTOs;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace api_iso_med_pg.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ScrumsController : ControllerBase
    {
        private readonly IScrumRepository _repository;
        private readonly IMapper _mapper;
        public ScrumsController(IScrumRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<ScrumDto>>>> Get()
        {
            try
            {
                var scrums = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ScrumDto>>(scrums);
                var reply = dtos != null && dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<ScrumDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<ScrumDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<ScrumDto>>> Get(int id)
        {
            try
            {
                var scrum = await _repository.GetByIdAsync(id);
                if (scrum == null)
                    return NotFound(new BaseResponse<ScrumDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<ScrumDto>(scrum);
                return Ok(new BaseResponse<ScrumDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ScrumDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateScrumDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreadoId = userId;
                }
                dto.FechaCreacion = DateTime.UtcNow;
                var scrum = _mapper.Map<Scrum>(dto);
                var created = await _repository.AddAsync(scrum);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_QUERY,
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
        public async Task<IActionResult> Put(UpdateScrumDto dto)
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
                    dto.ActualizadoId = userId;
                }
                dto.FechaActualizacion = DateTime.UtcNow;
                _mapper.Map(dto, existing);
                // Solo forzar UTC en fechas de actualización, no en delete
                existing.FechaActualizacion = DateTime.SpecifyKind(existing.FechaActualizacion ?? DateTime.UtcNow, DateTimeKind.Utc);
                existing.FechaCreacion = DateTime.SpecifyKind(existing.FechaCreacion ?? DateTime.UtcNow, DateTimeKind.Utc);
                await _repository.UpdateAsync(existing);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_UPDATE,
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
                // Obtener el id del usuario logueado desde el token
                var userIdClaim = User.FindFirst("id")?.Value;
                int deletedBy = 0;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    deletedBy = userId;
                }
                
                existing.Start = existing.Start.HasValue ? DateTime.SpecifyKind(existing.Start.Value, DateTimeKind.Utc) : (DateTime?)null;
                existing.End = existing.End.HasValue ? DateTime.SpecifyKind(existing.End.Value, DateTimeKind.Utc) : (DateTime?)null;
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
