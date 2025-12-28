namespace api_iso_med_pg.DTOs.Works;

public class UpdateWorkDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public decimal Cost { get; set; }
    public int Status { get; set; }
    public int ClientId { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
}
