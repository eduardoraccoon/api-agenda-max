using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories
{
    public class NormaRepository : GenericRepository<Norma>, INormaRepository
    {
        public NormaRepository(AppDbContext context) : base(context) { }
    }
}
