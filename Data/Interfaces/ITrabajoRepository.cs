using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Interfaces
{
    public interface ITrabajoRepository : IGenericRepository<Trabajo>
    {
        Task<IEnumerable<Trabajo>> GetAllWithClienteAsync();
        Task<Trabajo?> GetByIdWithClienteAsync(int id);
    }
}
