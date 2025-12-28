using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context) { }
    }
}
