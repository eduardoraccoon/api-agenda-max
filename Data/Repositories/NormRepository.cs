using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories
{
    public class NormRepository : GenericRepository<Norm>, INormRepository
    {
        public NormRepository(AppDbContext context) : base(context) { }
        // Métodos específicos si los necesitas
    }
}
