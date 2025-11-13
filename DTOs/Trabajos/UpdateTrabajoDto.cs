namespace api_iso_med_pg.DTOs.Trabajos;

public class UpdateTrabajoDto
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public decimal Costo { get; set; }
    public int Estatus { get; set; }
    public int ClienteId { get; set; }
    public int ActualizadoId { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public DateTime? FechaEntrega { get; set; }
}
