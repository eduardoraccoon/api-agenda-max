namespace api_iso_med_pg.DTOs;

public class CreateEntrevistaDto
{
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Ciudad { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
    public string? UrlArchivo { get; set; }
    public IFormFile? ArchivoCV { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}