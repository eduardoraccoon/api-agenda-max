namespace api_iso_med_pg.DTOs.Clients;

public class ClienteDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
}
