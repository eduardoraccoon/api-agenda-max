using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories;

public class CompaniaRepository : GenericRepository<Compania>, ICompaniaRepository
{
    public CompaniaRepository(AppDbContext context) : base(context) { }
}
