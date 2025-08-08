using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.DTOs.Sucursal;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api_iso_med_pg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalesController : ControllerBase
    {
        private readonly ISucursalRepository _repository;
        private readonly IMapper _mapper;
        public SucursalesController(ISucursalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<SucursalDto>>>> Get()
        {
            try
            {
                var sucursales = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<SucursalDto>>(sucursales);
                var reply = dtos != null && dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<SucursalDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<SucursalDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<SucursalDto>>> Get(int id)
        {
            try
            {
                var sucursal = await _repository.GetByIdAsync(id);
                if (sucursal == null)
                    return NotFound(new BaseResponse<SucursalDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<SucursalDto>(sucursal);
                return Ok(new BaseResponse<SucursalDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<SucursalDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("sucursales/{id}")]
        public async Task<ActionResult<BaseResponse<SucursalDto>>> GetByCompaniaId(int id)
        {
            try
            {
                var sucursales = await _repository.GetByCompaniaIdAsync(id);
                if (sucursales == null || !sucursales.Any())
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Data = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dtos = _mapper.Map<IEnumerable<SucursalDto>>(sucursales);
                return Ok(new BaseResponse<IEnumerable<SucursalDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<SucursalDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }
    }
}
