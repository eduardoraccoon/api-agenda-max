namespace api_iso_med_pg.DTOs.Pago;

public class UpdatePagoDto
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public int TrabajoId { get; set; }
    public int ActualizadoId { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public int MetodoPago { get; set; }
}
