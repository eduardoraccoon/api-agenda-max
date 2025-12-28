namespace api_iso_med_pg.DTOs.Clients;

public class CreateClientDto
{
    public string Name { get; set; } = null!;
    public string? Lastname { get; set; }
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
}
