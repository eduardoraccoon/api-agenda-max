using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.DTOs.Clients;
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
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _repository;
        private readonly IMapper _mapper;
        public ClientesController(IClienteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<ClienteDto>>>> Get()
        {
            try
            {
                var clients = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ClienteDto>>(clients);
                return Ok(new BaseResponse<IEnumerable<ClienteDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<ClienteDto>>
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<ClienteDto>>> Get(int id)
        {
            try
            {
                var client = await _repository.GetByIdAsync(id);
                if (client == null)
                    return NotFound(new BaseResponse<ClienteDto>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<ClienteDto>(client);
                return Ok(new BaseResponse<ClienteDto>
                {
                    IsSuccess = true,
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ClienteDto>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateClienteDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreadoId = userId;
                }
                dto.FechaCreacion = DateTime.UtcNow;
                var entity = _mapper.Map<Cliente>(dto);
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
        public async Task<IActionResult> Put(UpdateClienteDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing == null)
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.ActualizadoId = userId;
                }
                dto.FechaActualizacion = DateTime.UtcNow;
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
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                await _repository.DeleteAsync(id, userIdClaim != null ? int.Parse(userIdClaim) : 0);
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Message = ReplyMessage.MESSAGE_DELETE
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
