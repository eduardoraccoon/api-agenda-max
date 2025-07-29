namespace api_iso_med_pg.DTOs
{
    public class CreateNormDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
