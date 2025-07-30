namespace api_iso_med_pg.DTOs;

public class UpdateEntrevistaDto
{
    public int Id { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Ciudad { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? UrlImagen { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}