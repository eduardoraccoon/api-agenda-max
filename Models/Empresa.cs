namespace api_iso_med_pg.Models;

public class Empresa : BaseModel
{
    public string NombreComercial { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string Rfc { get; set; } = null!;
    public string RazonSocial { get; set; } = null!;
}
