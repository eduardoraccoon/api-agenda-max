namespace api_iso_med_pg.DTOs.Pago;

public class CreatePagoDto
{
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public int TrabajoId { get; set; }
    public int CreadoId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int MetodoPago { get; set; }
}
