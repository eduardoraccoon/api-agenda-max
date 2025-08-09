namespace api_iso_med_pg.Models;

public class Pregunta : BaseModel
{
    public string? Label { get; set; }
    public int TipoId { get; set; }
}
