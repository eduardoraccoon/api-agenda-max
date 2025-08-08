namespace api_iso_med_pg.DTOs
{
    public class UpdateScrumDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? ActualizadoId { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? Status { get; set; }
        public int? UserId { get; set; }
    }
}
