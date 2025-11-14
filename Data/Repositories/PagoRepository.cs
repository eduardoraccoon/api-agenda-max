using Microsoft.EntityFrameworkCore;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories
{
    public class PagoRepository : GenericRepository<Pago>, IPagoRepository
    {
        private readonly AppDbContext _db;
        public PagoRepository(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<Pago?> GetByIdWithTrabajoAsync(int id)
        {
            return await _db.Pagos
                .Include(p => p.Trabajo)
                .ThenInclude(t => t.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pago>> GetByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin)
        {
         return await _db.Pagos
             .Include(p => p.Trabajo)
             .ThenInclude(t => t.Cliente)
             .Where(p => p.FechaPago >= fechaInicio && p.FechaPago < fechaFin)
             .OrderByDescending(p => p.FechaPago)
             .ToListAsync();
        }
    }
}
