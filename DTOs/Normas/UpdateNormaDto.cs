namespace api_iso_med_pg.DTOs
{
    public class UpdateNormaDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? ActualizadoId { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
