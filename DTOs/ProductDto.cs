namespace api_iso_med_pg.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateProductDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
