namespace api_iso_med_pg.DTOs.Works;

public class CreateWorkDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Cost { get; set; }
    public int Status { get; set; }
    public int ClientId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
}
