namespace api_iso_med_pg.DTOs.Compania;

public class CreateCompaniaDto
{
    public string? Denominacion { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Acronimo { get; set; }
    public int? CreadoId { get; set; }
    public DateTime? FechaCreacion { get; set; }
}
