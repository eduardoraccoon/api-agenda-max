namespace api_iso_med_pg.DTOs.Payment;

public class UpdatePaymentDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int WorkId { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int PaymentMethod { get; set; }
}
