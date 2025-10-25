namespace api_iso_med_pg.DTOs.Trabajos;

public class TrabajoDto
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public decimal Costo { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; } = null!;
}
