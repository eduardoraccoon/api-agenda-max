namespace api_iso_med_pg.Models;

public class Work : BaseModel
{
    public string Description { get; set; } = null!;
    public decimal Cost { get; set; }
    public int Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public DateTime? DeliveryDate { get; set; } = null;
}
