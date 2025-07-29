using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.DTOs;
using api_iso_med_pg.Models;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api_iso_med_pg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<ProductDto>>>> Get()
        {
            try
            {
                var products = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
                var reply = dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
                return Ok(new BaseResponse<IEnumerable<ProductDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = reply
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<ProductDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<ProductDto>>> Get(int id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new BaseResponse<ProductDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<ProductDto>(product);
                return Ok(new BaseResponse<ProductDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ProductDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<ProductDto>>> Post(CreateProductDto dto)
        {
            try
            {
                var product = _mapper.Map<Product>(dto);
                var created = await _repository.AddAsync(product);
                var result = _mapper.Map<ProductDto>(created);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, new BaseResponse<ProductDto>
                {
                    IsSuccess = true,
                    Data = result,
                    Message = ReplyMessage.MESSAGE_SAVE
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ProductDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateProductDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(new BaseResponse<ProductDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_VALIDATE
                    });
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new BaseResponse<ProductDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                _mapper.Map(dto, existing);
                await _repository.UpdateAsync(existing);
                return Ok(new BaseResponse<ProductDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<ProductDto>(existing),
                    Message = ReplyMessage.MESSAGE_UPDATE
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ProductDto>
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
                    return NotFound(new BaseResponse<ProductDto>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                // Obtener el id del usuario logueado desde el token
                var userIdClaim = User.FindFirst("id")?.Value;
                int deletedBy = 0;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    deletedBy = userId;
                }
                await _repository.DeleteAsync(id, deletedBy);
                return Ok(new BaseResponse<ProductDto>
                {
                    IsSuccess = true,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_DELETE
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ProductDto>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ReplyMessage.MESSAGE_FAILED + $": {ex.Message}"
                });
            }
        }
    }
}
