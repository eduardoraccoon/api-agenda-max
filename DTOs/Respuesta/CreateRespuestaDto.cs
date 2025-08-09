namespace api_iso_med_pg.DTOs.Respuesta;

public class CreateRespuestaDto
{
    public string? ValorRespuesta { get; set; }
    public int PreguntaId { get; set; }
    public int UsuarioId { get; set; }
    public int TrabajadorId { get; set; }
    public int NoEvaluacion { get; set; }
    public int? CreadoId { get; set; }
    public DateTime? FechaCreacion { get; set; }
}
