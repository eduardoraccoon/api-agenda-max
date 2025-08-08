using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories;

public class SucursalRepository : GenericRepository<Sucursal>, ISucursalRepository
{
    public SucursalRepository(AppDbContext context) : base(context) { }

    public Task<IEnumerable<Sucursal>> GetByCompaniaIdAsync(int? companiaId = null)
    {
        if (companiaId == null)
        {
            return Task.FromResult(Enumerable.Empty<Sucursal>());
        }
        var result = _context.Set<Sucursal>().Where(s => s.CompaniaId == companiaId).AsEnumerable();
        return Task.FromResult(result);
    }
}
