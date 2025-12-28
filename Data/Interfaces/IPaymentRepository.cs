using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetByIdWithWorkAsync(int id);
        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
