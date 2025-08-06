namespace api_iso_med_pg.Models;

public class BaseModel
{
    public int Id { get; set; }
    public int? CreadoId { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int? ActualizadoId { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public int? EliminadoId { get; set; }
    public DateTime? FechaEliminacion { get; set; }
}
