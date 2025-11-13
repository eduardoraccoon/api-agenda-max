namespace api_iso_med_pg.Models;

public class Pago : BaseModel
{
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public int MetodoPago { get; set; }
    public int TrabajoId { get; set; }
    public Trabajo Trabajo { get; set; } = null!;
}
