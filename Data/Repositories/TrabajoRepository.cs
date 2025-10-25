using Microsoft.EntityFrameworkCore;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories
{
    public class TrabajoRepository : GenericRepository<Trabajo>, ITrabajoRepository
    {
        private readonly AppDbContext _db;
        public TrabajoRepository(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Trabajo>> GetAllWithClienteAsync()
        {
            return await _db.Trabajos.Include(t => t.Cliente).ToListAsync();
        }

        public async Task<Trabajo?> GetByIdWithClienteAsync(int id)
        {
            return await _db.Trabajos.Include(t => t.Cliente).FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
