using Microsoft.EntityFrameworkCore;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var deletedAtProperty = typeof(T).GetProperty("DeletedAt");
            var idProperty = typeof(T).GetProperty("Id");
            IQueryable<T> query = _dbSet;
            if (deletedAtProperty != null)
            {
                query = query.Where(e => EF.Property<DateTime?>(e, "DeletedAt") == null);
            }
            if (idProperty != null)
            {
                query = query.OrderByDescending(e => EF.Property<object>(e, "Id"));
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, int deletedBy)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                var deletedByProp = entity.GetType().GetProperty("DeletedBy");
                var deletedAtProp = entity.GetType().GetProperty("DeletedAt");
                if (deletedByProp != null)
                {
                    deletedByProp.SetValue(entity, deletedBy);
                }
                if (deletedAtProp != null)
                {
                    deletedAtProp.SetValue(entity, DateTime.UtcNow);
                }
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
