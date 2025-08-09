namespace api_iso_med_pg.DTOs.Pregunta;

public class UpdatePreguntaDto
{
    public int Id { get; set; }
    public string? Label { get; set; }
    public int TipoId { get; set; }
    public int? ActualizadoId { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
