using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Interfaces
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        Task<IEnumerable<Work>> GetAllWithClienteAsync();
        Task<Work?> GetByIdWithClienteAsync(int id);
    }
}
