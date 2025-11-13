namespace api_iso_med_pg.Models;

public class Trabajo : BaseModel
{
    public string Descripcion { get; set; } = null!;
    public decimal Costo { get; set; }
    public int Estatus { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public DateTime? FechaEntrega { get; set; } = null;
}
