using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Interfaces
{
    public interface IPagoRepository : IGenericRepository<Pago>
    {
        Task<Pago?> GetByIdWithTrabajoAsync(int id);
        Task<IEnumerable<Pago>> GetByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}
