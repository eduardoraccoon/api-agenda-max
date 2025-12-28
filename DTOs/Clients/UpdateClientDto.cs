namespace api_iso_med_pg.DTOs.Clients;

public class UpdateClientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
