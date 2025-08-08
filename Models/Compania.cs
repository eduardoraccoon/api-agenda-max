namespace api_iso_med_pg.Models;

public class Compania : BaseModel
{
    public string? Denominacion { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Acronimo { get; set; }
}