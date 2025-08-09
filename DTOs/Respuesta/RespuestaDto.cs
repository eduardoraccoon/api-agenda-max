namespace api_iso_med_pg.DTOs.Respuesta;

public class RespuestaDto
{
    public int Id { get; set; }
    public string? ValorRespuesta { get; set; }
    public int PreguntaId { get; set; }
    public int UsuarioId { get; set; }
    public int TrabajadorId { get; set; }
    public string? NoEvaluacion { get; set; }
}
