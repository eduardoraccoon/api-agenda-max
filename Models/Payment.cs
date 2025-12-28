namespace api_iso_med_pg.Models;

public class Payment : BaseModel
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PaymentMethod { get; set; }
    public int WorkId { get; set; }
    public Work Work { get; set; } = null!;
}
