using Microsoft.EntityFrameworkCore;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly AppDbContext _db;
        public PaymentRepository(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<Payment?> GetByIdWithWorkAsync(int id)
        {
            return await _db.Payments
                .Include(p => p.Work)
                .ThenInclude(t => t.Client)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
         return await _db.Payments
             .Include(p => p.Work)
             .ThenInclude(t => t.Client)
             .Where(p => p.PaymentDate >= startDate && p.PaymentDate < endDate)
             .OrderByDescending(p => p.PaymentDate)
             .ToListAsync();
        }
    }
}
