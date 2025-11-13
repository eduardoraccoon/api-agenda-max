namespace api_iso_med_pg.DTOs.Trabajos;

public class CreateTrabajoDto
{
    public string Descripcion { get; set; } = null!;
    public decimal Costo { get; set; }
    public int Estatus { get; set; }
    public int ClienteId { get; set; }
    public int CreadoId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public DateTime? FechaEntrega { get; set; }
}
