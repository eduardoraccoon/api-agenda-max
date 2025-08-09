namespace api_iso_med_pg.DTOs.Respuesta;

public class UpdateRespuestaDto
{
    public int Id { get; set; }
    public string? ValorRespuesta { get; set; }
    public int PreguntaId { get; set; }
    public int UsuarioId { get; set; }
    public int TrabajadorId { get; set; }
    public int NoEvaluacion { get; set; }
    public int? ActualizadoId { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
