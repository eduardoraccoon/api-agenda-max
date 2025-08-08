using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Interfaces;

public interface ISucursalRepository : IGenericRepository<Sucursal>
{
    Task<IEnumerable<Sucursal>> GetByCompaniaIdAsync(int? companiaId = null);
}
