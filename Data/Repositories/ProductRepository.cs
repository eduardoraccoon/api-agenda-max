using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }
    }
}
