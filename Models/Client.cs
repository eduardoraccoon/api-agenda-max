namespace api_iso_med_pg.Models;

public class Client : BaseModel
{
    public string Name { get; set; } = null!;
    public string? Lastname { get; set; }
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
}
