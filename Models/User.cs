namespace api_iso_med_pg.Models;

public class User
{
    public int Id { get; set; }
    public required string Usuario { get; set; }
    public required string PasswordHash { get; set; }
    public int TrabajadorId { get; set; }
    public Trabajador? Trabajador { get; set; }
}
