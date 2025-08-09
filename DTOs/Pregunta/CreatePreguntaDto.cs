namespace api_iso_med_pg.DTOs.Pregunta;

public class CreatePreguntaDto
{
    public string? Label { get; set; }
    public int TipoId { get; set; }
    public int? CreadoId { get; set; }
    public DateTime? FechaCreacion { get; set; }
}
