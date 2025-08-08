using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.DTOs.Compania;
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
    public class CompaniasController : ControllerBase
    {
        private readonly ICompaniaRepository _repository;
        private readonly IMapper _mapper;
        public CompaniasController(ICompaniaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<CompaniaDto>>>> Get()
        {
            try
            {
                var companias = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<CompaniaDto>>(companias);
                var reply = dtos != null && dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<CompaniaDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<CompaniaDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<CompaniaDto>>> Get(int id)
        {
            try
            {
                var compania = await _repository.GetByIdAsync(id);
                if (compania == null)
                    return NotFound(new BaseResponse<CompaniaDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<CompaniaDto>(compania);
                return Ok(new BaseResponse<CompaniaDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<CompaniaDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateCompaniaDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreadoId = userId;
                }
                dto.FechaCreacion = DateTime.UtcNow;
                var compania = _mapper.Map<Compania>(dto);
                var created = await _repository.AddAsync(compania);
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

        [HttpPut]
        public async Task<IActionResult> Put(UpdateCompaniaDto dto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing == null)
                {
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Data = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                }

                var userIdClaim = User.FindFirst("id")?.Value;
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
                    Data = ReplyMessage.MESSAGE_DELETE
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
