namespace api_iso_med_pg.Models;

public class Equipamiento : BaseModel
{
    public string? Nombre { get; set; }
    public string? Codigo { get; set; }
    public string? Organizacion { get; set; }
    public string? Estado { get; set; }
    public string? Modelo { get; set; }
    public string? Serie { get; set; }
    public DateTime? FechaAsignacion { get; set; }
    public string? SucursalId { get; set; }
    public string? PuestoId { get; set; }
    public string? Propiedad { get; set; }
    public string? TipoEquipo { get; set; }
    public string? Tipo { get; set; }
    public string? UsoEquipo { get; set; }
    public string? UsoId { get; set; }
    public string? DetallesUso { get; set; }
    public string? Requerimientos { get; set; }
    public string? UrlImagen { get; set; }
    public string? NumeroEconomico { get; set; }
}
