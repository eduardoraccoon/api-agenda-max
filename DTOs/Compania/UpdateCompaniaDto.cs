namespace api_iso_med_pg.DTOs.Compania;

public class UpdateCompaniaDto
{
    public int Id { get; set; }
    public string? Denominacion { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Acronimo { get; set; }
    public int? ActualizadoId { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
