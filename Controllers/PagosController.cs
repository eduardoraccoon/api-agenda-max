using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.DTOs.Pago;
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
    public class PagosController : ControllerBase
    {
        private readonly IPagoRepository _repository;
        private readonly IMapper _mapper;
        public PagosController(IPagoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private DateTime AjustarHoraSiSoloFecha(DateTime fecha)
        {
            // Si la hora es 00:00:00, asignar 01:00:00
            if (fecha.Hour == 0 && fecha.Minute == 0 && fecha.Second == 0)
                return new DateTime(fecha.Year, fecha.Month, fecha.Day, 1, 0, 0, DateTimeKind.Utc);
            return fecha;
        }
        [HttpGet("pdf/{id}")]
        public async Task<IActionResult> GetPagoPdf(int id)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var pago = await _repository.GetByIdWithTrabajoAsync(id);
            if (pago == null)
                return NotFound("Pago no encontrado");

            // Obtener la descripción del trabajo y el nombre del cliente
            var trabajoDescripcion = pago.Trabajo?.Descripcion ?? "Sin descripción";
            var trabajoId = pago.Trabajo?.Id;
            var clienteNombre = pago.Trabajo?.Cliente?.Nombre ?? "Sin cliente";

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
                        col.Item().Text($"Trabajo: T-00{trabajoId}");
                        col.Item().Text($"Trabajo: {trabajoDescripcion}");
                        col.Item().Text($"Cliente: {clienteNombre}");
                        col.Item().Text($"Monto: {pago.Monto:C}");
                        col.Item().Text($"Fecha de Pago: {pago.FechaPago:dd/MM/yyyy}");
                    });
                });
            })
            .GeneratePdf(stream);
            stream.Position = 0;
            return File(stream, "application/pdf", $"recibo_pago_{pago.Id}.pdf");
        }
        

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<PagoDto>>>> Get()
        {
            try
            {
                var pagos = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PagoDto>>(pagos);
                return Ok(new BaseResponse<IEnumerable<PagoDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<PagoDto>>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpGet("{fechaInicio}/{fechaFin}")]
        public async Task<ActionResult<BaseResponse<IEnumerable<PagoDto>>>> Get(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var pagos = await _repository.GetByDateRangeAsync(fechaInicio, fechaFin);
                var dtos = _mapper.Map<IEnumerable<PagoDto>>(pagos);
                return Ok(new BaseResponse<IEnumerable<PagoDto>>
                {
                    IsSuccess = true,
                    Data = dtos,
                    Message = dtos.Any() ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<PagoDto>>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<PagoDto>>> Get(int id)
        {
            try
            {
                var pago = await _repository.GetByIdAsync(id);
                if (pago == null)
                    return NotFound(new BaseResponse<PagoDto>
                    {
                        IsSuccess = false,
                        Message = ReplyMessage.MESSAGE_QUERY_EMPTY
                    });
                var dto = _mapper.Map<PagoDto>(pago);
                return Ok(new BaseResponse<PagoDto>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message = ReplyMessage.MESSAGE_QUERY
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<PagoDto>
                {
                    IsSuccess = false,
                    Message = $"{ReplyMessage.MESSAGE_FAILED}: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> Post(CreatePagoDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.CreadoId = userId;
                }
                dto.FechaCreacion = DateTime.UtcNow;
                var fecha = DateTime.SpecifyKind(dto.FechaPago, DateTimeKind.Utc);
                dto.FechaPago = AjustarHoraSiSoloFecha(fecha);
                var entity = _mapper.Map<Pago>(dto);
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
        public async Task<IActionResult> Put(UpdatePagoDto dto)
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
