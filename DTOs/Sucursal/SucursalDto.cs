namespace api_iso_med_pg.DTOs.Sucursal;

public class SucursalDto
{
    public int Id { get; set; }
    public string? Suc { get; set; }
    public string? Nomenclatura { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public int? CompaniaId { get; set; }
}
