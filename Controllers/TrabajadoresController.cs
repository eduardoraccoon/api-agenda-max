using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.DTOs.Trabajador;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api_iso_med_pg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrabajadoresController : ControllerBase
    {
        private readonly ITrabajadorRepository _trabajadorRepository;
        private readonly IMapper _mapper;

        public TrabajadoresController(ITrabajadorRepository trabajadorRepository, IMapper mapper)
        {
            _trabajadorRepository = trabajadorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var trabajadores = await _trabajadorRepository.GetAllAsync();
                if (trabajadores == null || !trabajadores.Any())
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Data = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dtos = _mapper.Map<IEnumerable<TrabajadorDto>>(trabajadores);
                return Ok(new BaseResponse<IEnumerable<TrabajadorDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<TrabajadorDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var trabajador = await _trabajadorRepository.GetByIdAsync(id);
                if (trabajador == null)
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Data = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<TrabajadorDto>(trabajador);
                return Ok(new BaseResponse<TrabajadorDto>
                {
                    IsSuccess = true,
                    Data = dto,
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
    }
}
