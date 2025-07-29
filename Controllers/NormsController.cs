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
    public class NormsController : ControllerBase
    {
        private readonly INormRepository _repository;
        private readonly IMapper _mapper;
        public NormsController(INormRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<NormDto>>>> Get()
        {
            try
            {
                var norms = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<NormDto>>(norms);
                var reply = dtos != null && dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<NormDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<NormDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<NormDto>>> Get(int id)
        {
            try
            {
                var norm = await _repository.GetByIdAsync(id);
                if (norm == null)
                    return NotFound(new BaseResponse<NormDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<NormDto>(norm);
                return Ok(new BaseResponse<NormDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<NormDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateNormDto dto)
        {
            try
            {
                // Obtener el id del usuario logueado desde el token
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreatedBy = userId;
                }
                dto.CreatedAt = DateTime.UtcNow;
                var norm = _mapper.Map<Norm>(dto);
                var created = await _repository.AddAsync(norm);
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
        public async Task<IActionResult> Put(UpdateNormDto dto)
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
                // Obtener el id del usuario logueado desde el token
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.UpdatedBy = userId;
                }
                dto.UpdatedAt = DateTime.UtcNow;
                _mapper.Map(dto, existing);
                // Forzar Kind=Utc en fechas relevantes de forma simplificada
                existing.UpdatedAt = DateTime.SpecifyKind(existing.UpdatedAt ?? DateTime.UtcNow, DateTimeKind.Utc);
                existing.CreatedAt = DateTime.SpecifyKind(existing.CreatedAt ?? DateTime.UtcNow, DateTimeKind.Utc);
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
                // Obtener el id del usuario logueado desde el token
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
