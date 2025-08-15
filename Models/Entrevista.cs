namespace api_iso_med_pg.Models;

public class Entrevista : BaseModel
{
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Ciudad { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? UrlArchivo { get; set; }
}
