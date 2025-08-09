namespace api_iso_med_pg.Models;

public class Respuesta : BaseModel
{
    public string? ValorRespuesta { get; set; }
    public int PreguntaId { get; set; }
    public int UsuarioId { get; set; }
    public int TrabajadorId { get; set; }
    public string? NoEvaluacion { get; set; }
    public Pregunta? Pregunta { get; set; }
    public User? User { get; set; }
}
