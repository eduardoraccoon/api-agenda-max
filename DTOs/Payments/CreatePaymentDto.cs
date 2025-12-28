namespace api_iso_med_pg.DTOs.Payment;

public class CreatePaymentDto
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int WorkId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int PaymentMethod { get; set; }
}
