using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.DTOs.Empresa;
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
    public class EmpresasController : ControllerBase
    {
        private readonly IEmpresaRepository _repository;
        private readonly IMapper _mapper;
        public EmpresasController(IEmpresaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<EmpresaDto>>>> Get()
        {
            try
            {
                var empresas = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EmpresaDto>>(empresas);
                return Ok(new BaseResponse<IEnumerable<EmpresaDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<EmpresaDto>>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<EmpresaDto>>> Get(int id)
        {
            try
            {
                var empresa = await _repository.GetByIdAsync(id);
                if (empresa == null)
                    return NotFound(new BaseResponse<EmpresaDto>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<EmpresaDto>(empresa);
                return Ok(new BaseResponse<EmpresaDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<EmpresaDto>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateEmpresaDto dto)
        {
            try
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out int userId))
                {
                    dto.CreadoId = userId;
                }
                dto.FechaCreacion = DateTime.UtcNow;
                var entity = _mapper.Map<Empresa>(dto);
                await _repository.AddAsync(entity);
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
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateEmpresaDto dto)
        {
            try
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out int userId))
                {
                    dto.ActualizadoId = userId;
                }
                dto.FechaActualizacion = DateTime.UtcNow;
                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing == null)
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_UPDATE
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
                return StatusCode(500, new BaseResponse<string>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
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
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }
    }
}
