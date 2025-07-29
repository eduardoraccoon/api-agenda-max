namespace api_iso_med_pg.DTOs
{
    public class CreateScrumDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public int? UserId { get; set; }
    }
}
