namespace api_iso_med_pg.Models;

public class Sucursal
{
    public int Id { get; set; }
    public string? Suc { get; set; }
    public string? Nomenclatura { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Denominacion { get; set; }
    public int? CompaniaId { get; set; }
}
