namespace api_iso_med_pg.Models;

public class Scrum : BaseModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string? Status { get; set; }
    public int? UserId { get; set; }
}
