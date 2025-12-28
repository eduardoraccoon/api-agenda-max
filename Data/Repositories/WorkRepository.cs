using Microsoft.EntityFrameworkCore;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories
{
    public class WorkRepository : GenericRepository<Work>, IWorkRepository
    {
        private readonly AppDbContext _db;
        public WorkRepository(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Work>> GetAllWithClienteAsync()
        {
            return await _db.Works
                .Include(t => t.Client)
                .Where(t => t.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Work?> GetByIdWithClienteAsync(int id)
        {
            return await _db.Works
                .Include(t => t.Client)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
