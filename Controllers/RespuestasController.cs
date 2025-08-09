using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.DTOs.Respuesta;
using api_iso_med_pg.Models;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_iso_med_pg.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class RespuestasController : ControllerBase
    {
        private readonly IRespuestaRepository _repository;
        private readonly IMapper _mapper;
        public RespuestasController(IRespuestaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<RespuestaDto>>>> Get()
        {
            try
            {
                var respuestas = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<RespuestaDto>>(respuestas);
                var reply = dtos != null && dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<RespuestaDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<RespuestaDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<RespuestaDto>>> Get(int id)
        {
            try
            {
                var respuesta = await _repository.GetByIdAsync(id);
                if (respuesta == null)
                    return NotFound(new BaseResponse<RespuestaDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<RespuestaDto>(respuesta);
                return Ok(new BaseResponse<RespuestaDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<RespuestaDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateRespuestaDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreadoId = userId;
                }
                dto.FechaCreacion = DateTime.UtcNow;
                var respuesta = _mapper.Map<Respuesta>(dto);
                var created = await _repository.AddAsync(respuesta);
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
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPost("evaluacion")]
        public async Task<ActionResult<BaseResponse<string>>> PostBulk([FromBody] EncuestaRespuestaDto dto)
        {
            if (dto.Responses == null || string.IsNullOrEmpty(dto.IdWorker))
                return BadRequest(new BaseResponse<string>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Datos incompletos"
                });
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                string guid = Guid.NewGuid().ToString("N");
                int creadoId = 0;
                if (int.TryParse(userIdClaim, out int userId))
                    creadoId = userId;

                var respuestasList = new List<Respuesta>();
                foreach (var kvp in dto.Responses)
                {
                    respuestasList.Add(new Respuesta
                    {
                        PreguntaId = int.Parse(kvp.Key),
                        ValorRespuesta = kvp.Value,
                        TrabajadorId = int.Parse(dto.IdWorker),
                        CreadoId = creadoId,
                        FechaCreacion = DateTime.UtcNow,
                        NoEvaluacion = guid
                    });
                }
                foreach (var respuesta in respuestasList)
                {
                    await _repository.AddAsync(respuesta);
                }
                return Ok(new BaseResponse<string>
                {
                    IsSuccess = true,
                    Data = "Respuestas guardadas correctamente"
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

        [HttpGet("table-evaluations/{workerId}/{dateStart}/{dateEnd}")]
        public async Task<BaseResponse<IEnumerable<GetEvaluationsDto>>> GetTableEvaluations(int workerId, string dateStart, string dateEnd)
        {
            try
            {
                var dateStartUtc = DateTime.SpecifyKind(DateTime.Parse(dateStart), DateTimeKind.Utc);
                var dateEndUtc = DateTime.SpecifyKind(DateTime.Parse(dateEnd), DateTimeKind.Utc);
                var evaluations = await _repository.GetTableEvaluationsAsync(workerId, dateStartUtc, dateEndUtc);

                return new BaseResponse<IEnumerable<GetEvaluationsDto>>
                {
                    IsSuccess = true,
                    Data = evaluations!
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GetEvaluationsDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
