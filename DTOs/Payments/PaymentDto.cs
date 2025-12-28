namespace api_iso_med_pg.DTOs.Payment;

public class PaymentDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int WorkId { get; set; }
    public string WorkDescription { get; set; } = null!;
    public int PaymentMethod { get; set; }
}
