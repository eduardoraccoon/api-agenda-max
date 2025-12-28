namespace api_iso_med_pg.DTOs.Works;

public class WorkDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public decimal Cost { get; set; }
    public int Status { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = null!;
    public DateTime? DeliveryDate { get; set; } = null;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
