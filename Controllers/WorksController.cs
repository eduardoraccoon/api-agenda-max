using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using api_iso_med_pg.DTOs.Works;

namespace api_iso_med_pg.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class WorksController : ControllerBase
    {


        private DateTime? NormalizarFecha(DateTime? fecha)
        {
            if (!fecha.HasValue) return null;
            var f = fecha.Value;
            // Si los segundos son 0 y el string original no los tenía, igualar a formato completo
            if (f.Second == 0 && f.Millisecond == 0)
                return new DateTime(f.Year, f.Month, f.Day, f.Hour, f.Minute, 0, DateTimeKind.Unspecified);
            return DateTime.SpecifyKind(f, DateTimeKind.Unspecified);
        }
        private readonly IWorkRepository _repository;
        private readonly IMapper _mapper;
        public WorksController(IWorkRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<WorkDto>>>> Get()
        {
            try
            {
                var trabajos = await _repository.GetAllWithClienteAsync();
                var dtos = _mapper.Map<IEnumerable<WorkDto>>(trabajos);
                return Ok(new BaseResponse<IEnumerable<WorkDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? "Trabajos encontrados" : "No hay trabajos registrados"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<WorkDto>>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<WorkDto>>> Get(int id)
        {
            try
            {
                var work = await _repository.GetByIdWithClienteAsync(id);
                if (work == null)
                    return NotFound(new BaseResponse<WorkDto>
                    {
                        IsSuccess = false,
                        Message = "Trabajo no encontrado"
                    });
                var dto = _mapper.Map<WorkDto>(work);
                return Ok(new BaseResponse<WorkDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = "Trabajo encontrado"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<WorkDto>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateWorkDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreatedBy = userId;
                }
                dto.CreatedAt = DateTime.UtcNow;

                // Guardar fechas como UTC si existen, si no, dejar null
                dto.StartDate = dto.StartDate.HasValue ? DateTime.SpecifyKind(dto.StartDate.Value, DateTimeKind.Utc) : null;
                dto.EndDate = dto.EndDate.HasValue ? DateTime.SpecifyKind(dto.EndDate.Value, DateTimeKind.Utc) : null;
                dto.DeliveryDate = dto.DeliveryDate.HasValue ? DateTime.SpecifyKind(dto.DeliveryDate.Value, DateTimeKind.Utc) : null;
                // No se requiere validación obligatoria para fechas

                var entity = _mapper.Map<Work>(dto);
                await _repository.AddAsync(entity);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_SAVE
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateWorkDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.UpdatedBy = userId;
                }
                dto.UpdatedAt = DateTime.UtcNow;

                // Guardar fechas como UTC para compatibilidad con PostgreSQL
                dto.StartDate = dto.StartDate.HasValue ? DateTime.SpecifyKind(dto.StartDate.Value, DateTimeKind.Utc) : null;
                dto.EndDate = dto.EndDate.HasValue ? DateTime.SpecifyKind(dto.EndDate.Value, DateTimeKind.Utc) : null;
                dto.DeliveryDate = dto.DeliveryDate.HasValue ? DateTime.SpecifyKind(dto.DeliveryDate.Value, DateTimeKind.Utc) : null;


                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing == null)
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Message =ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                _mapper.Map(dto, existing);
                await _repository.UpdateAsync(existing);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_UPDATE
                });
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException != null ? ex.InnerException.Message : "";
                return StatusCode(500, new BaseResponse<string>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} | Inner: {inner}"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                await _repository.DeleteAsync(id, userIdClaim != null ? int.Parse(userIdClaim) : 0);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = ReplyMessage.MESSAGE_DELETE
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }
    }
}
