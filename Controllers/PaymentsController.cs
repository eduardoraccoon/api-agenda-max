using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using api_iso_med_pg.DTOs.Payment;

namespace api_iso_med_pg.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;
        public PaymentsController(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("pdf/{id}")]
        public async Task<IActionResult> GetPagoPdf(int id)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var payment = await _repository.GetByIdWithWorkAsync(id);
            if (payment == null)
                return NotFound("Pago no encontrado");

            // Obtener la descripción del trabajo y el nombre del cliente
            var workDescription = payment.Work?.Description ?? "Sin descripción";
            var workId = payment.Work?.Id;
            var clientName = payment.Work?.Client?.Name ?? "Sin cliente";

            // Generar PDF con QuestPDF
            var stream = new MemoryStream();
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content().Column(col =>
                    {
                        // Insertar imagen al inicio usando el nuevo overload
                        col.Item().Image(Image.FromFile("Files/Logos/logo-bandera.png")).FitWidth();
                        col.Item().Text("Recibo de Pago").FontSize(20).Bold();
                        col.Item().Text($"Trabajo: T-00{workId}");
                        col.Item().Text($"Trabajo: {workDescription}");
                        col.Item().Text($"Cliente: {clientName}");
                        col.Item().Text($"Monto: {payment.Amount:C}");
                        col.Item().Text($"Fecha de Pago: {payment.PaymentDate:dd/MM/yyyy}");
                    });
                });
            })
            .GeneratePdf(stream);
            stream.Position = 0;
            return File(stream, "application/pdf", $"recibo_pago_{payment.Id}.pdf");
        }
        

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<PaymentDto>>>> Get()
        {
            try
            {
                var payments = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PaymentDto>>(payments);
                return Ok(new BaseResponse<IEnumerable<PaymentDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<PaymentDto>>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpGet("{dateStart}/{dateEnd}")]
        public async Task<ActionResult<BaseResponse<IEnumerable<PaymentDto>>>> Get(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var payments = await _repository.GetByDateRangeAsync(dateStart, dateEnd);
                var dtos = _mapper.Map<IEnumerable<PaymentDto>>(payments);
                return Ok(new BaseResponse<IEnumerable<PaymentDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<PaymentDto>>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<PaymentDto>>> Get(int id)
        {
            try
            {
                var payment = await _repository.GetByIdAsync(id);
                if (payment == null)
                    return NotFound(new BaseResponse<PaymentDto>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<PaymentDto>(payment);
                return Ok(new BaseResponse<PaymentDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<PaymentDto>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreatePaymentDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreatedBy = userId;
                }
                dto.CreatedAt = DateTime.UtcNow;
                var entity = _mapper.Map<Payment>(dto);
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
        public async Task<IActionResult> Put(UpdatePaymentDto dto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(dto.Id);
                if (existing == null)
                    return NotFound(new BaseResponse<string>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.UpdatedBy = userId;
                }
                dto.UpdatedAt = DateTime.UtcNow;
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
                await _repository.DeleteAsync(id, userIdClaim != null ? int.Parse(userIdClaim) : (int)0);
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
