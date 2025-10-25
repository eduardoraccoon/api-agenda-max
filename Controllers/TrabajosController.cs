using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.DTOs.Trabajos;
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
    public class TrabajosController : ControllerBase
    {
        private readonly ITrabajoRepository _repository;
        private readonly IMapper _mapper;
        public TrabajosController(ITrabajoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<TrabajoDto>>>> Get()
        {
            try
            {
                var trabajos = await _repository.GetAllWithClienteAsync();
                var dtos = _mapper.Map<IEnumerable<TrabajoDto>>(trabajos);
                return Ok(new BaseResponse<IEnumerable<TrabajoDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? "Trabajos encontrados" : "No hay trabajos registrados"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<TrabajoDto>>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<TrabajoDto>>> Get(int id)
        {
            try
            {
                var trabajo = await _repository.GetByIdWithClienteAsync(id);
                if (trabajo == null)
                    return NotFound(new BaseResponse<TrabajoDto>
                    {
                        IsSuccess = false,
                        Message = "Trabajo no encontrado"
                    });
                var dto = _mapper.Map<TrabajoDto>(trabajo);
                return Ok(new BaseResponse<TrabajoDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = "Trabajo encontrado"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<TrabajoDto>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateTrabajoDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreadoId = userId;
                }
                dto.FechaCreacion = DateTime.UtcNow;
                var entity = _mapper.Map<Trabajo>(dto);
                await _repository.AddAsync(entity);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = entity.Id.ToString(),
                    Message = "Trabajo creado exitosamente"
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
        public async Task<IActionResult> Put(UpdateTrabajoDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.ActualizadoId = userId;
                }
                dto.FechaActualizacion = DateTime.UtcNow;
                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing == null)
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Message = "Trabajo no encontrado"
                    });
                _mapper.Map(dto, existing);
                await _repository.UpdateAsync(existing);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Message = "Trabajo actualizado exitosamente"
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
                        Message = "Trabajo no encontrado"
                    });
                await _repository.DeleteAsync(id, userIdClaim != null ? int.Parse(userIdClaim) : 0);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Message = "Trabajo eliminado exitosamente"
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
