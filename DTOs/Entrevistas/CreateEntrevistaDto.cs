namespace api_iso_med_pg.DTOs;

public class CreateEntrevistaDto
{
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Ciudad { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? UrlImagen { get; set; }
    public IFormFile? ArchivoImagen { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}