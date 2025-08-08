namespace api_iso_med_pg.DTOs
{
    public class CreateNormaDto
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? CreadoId { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
