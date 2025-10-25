namespace api_iso_med_pg.DTOs.Clients;

public class UpdateClienteDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
     public int? ActualizadoId { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
