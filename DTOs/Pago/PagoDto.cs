namespace api_iso_med_pg.DTOs.Pago;

public class PagoDto
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public int TrabajoId { get; set; }
    public string TrabajoDescripcion { get; set; } = null!;
    public int MetodoPago { get; set; }
}
