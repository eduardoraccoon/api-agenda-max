namespace api_iso_med_pg.DTOs.Clients;

public class CreateClienteDto
{
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public int? CreadoId { get; set; }
    public DateTime? FechaCreacion { get; set; }
}
