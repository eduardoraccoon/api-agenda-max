using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.DTOs.Pregunta;
using api_iso_med_pg.Models;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api_iso_med_pg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreguntasController : ControllerBase
    {
        private readonly IPreguntaRepository _preguntaRepository;
        private readonly IMapper _mapper;

        public PreguntasController(IPreguntaRepository preguntaRepository, IMapper mapper)
        {
            _preguntaRepository = preguntaRepository;
            _mapper = mapper;
        }

        // GET: api/Preguntas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreguntaDto>>> GetAll()
        {
            try
            {
                var preguntas = await _preguntaRepository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PreguntaDto>>(preguntas);
                var reply = dtos != null && dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<PreguntaDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<PreguntaDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        // GET: api/Preguntas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PreguntaDto>> GetById(int id)
        {
            try
            {
                var pregunta = await _preguntaRepository.GetByIdAsync(id);
                if (pregunta == null)
                    return NotFound(new BaseResponse<PreguntaDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<PreguntaDto>(pregunta);
                return Ok(new BaseResponse<PreguntaDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<PreguntaDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        // POST: api/Preguntas
        //     [HttpPost]
        //     public async Task<ActionResult<PreguntaDto>> Create(CreatePreguntaDto createPreguntaDto)
        //     {
        //         var pregunta = _mapper.Map<Pregunta>(createPreguntaDto);
        //         await _preguntaRepository.AddAsync(pregunta);

        //         var preguntaDto = _mapper.Map<PreguntaDto>(pregunta);
        //         return CreatedAtAction(nameof(GetById), new { id = pregunta.Id }, preguntaDto);
        //     }

        //     // PUT: api/Preguntas/5
        //     [HttpPut("{id}")]
        //     public async Task<IActionResult> Update(int id, UpdatePreguntaDto updatePreguntaDto)
        //     {
        //         if (id != updatePreguntaDto.Id)
        //         {
        //             return BadRequest();
        //         }

        //         var existingPregunta = await _preguntaRepository.GetByIdAsync(id);
        //         if (existingPregunta == null)
        //         {
        //             return NotFound();
        //         }

        //         _mapper.Map(updatePreguntaDto, existingPregunta);
        //         _preguntaRepository.Update(existingPregunta);

        //         return NoContent();
        //     }

        //     // DELETE: api/Preguntas/5
        //     [HttpDelete("{id}")]
        //     public async Task<IActionResult> Delete(int id)
        //     {
        //         var existingPregunta = await _preguntaRepository.GetByIdAsync(id);
        //         if (existingPregunta == null)
        //         {
        //             return NotFound();
        //         }

        //         _preguntaRepository.Delete(existingPregunta);
        //         return NoContent();
        //     }
    }
}