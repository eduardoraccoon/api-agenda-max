using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using api_iso_med_pg.DTOs.Companies;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;
        public CompaniesController(ICompanyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<CompanyDto>>>> Get()
        {
            try
            {
                var companies = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                return Ok(new BaseResponse<IEnumerable<CompanyDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<CompanyDto>>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<CompanyDto>>> Get(int id)
        {
            try
            {
                var company = await _repository.GetByIdAsync(id);
                if (company == null)
                    return NotFound(new BaseResponse<CompanyDto>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<CompanyDto>(company);
                return Ok(new BaseResponse<CompanyDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<CompanyDto>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreateCompanyDto dto)
        {
            try
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out int userId))
                {
                    dto.CreatedBy = userId;
                }
                dto.CreatedAt = DateTime.UtcNow;
                var entity = _mapper.Map<Company>(dto);
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
        public async Task<IActionResult> Put(UpdateCompanyDto dto)
        {
            try
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out int userId))
                {
                    dto.UpdatedBy = userId;
                }
                dto.UpdatedAt = DateTime.UtcNow;
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
