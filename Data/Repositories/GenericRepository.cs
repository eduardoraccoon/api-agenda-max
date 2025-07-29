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
            // Forzar UTC en propiedades UpdatedAt y CreatedAt si existen
            var nowUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            var updatedAtProp = entity.GetType().GetProperty("UpdatedAt");
            var createdAtProp = entity.GetType().GetProperty("CreatedAt");
            if (updatedAtProp != null)
            {
                var val = updatedAtProp.GetValue(entity) as DateTime?;
                updatedAtProp.SetValue(entity, DateTime.SpecifyKind(val ?? nowUtc, DateTimeKind.Utc));
            }
            if (createdAtProp != null)
            {
                var val = createdAtProp.GetValue(entity) as DateTime?;
                createdAtProp.SetValue(entity, DateTime.SpecifyKind(val ?? nowUtc, DateTimeKind.Utc));
            }
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, int deletedBy)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                // Forzar UTC en propiedades DeletedAt, UpdatedAt, CreatedAt si existen
                var nowUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                var deletedAtProp = entity.GetType().GetProperty("DeletedAt");
                var updatedAtProp = entity.GetType().GetProperty("UpdatedAt");
                var createdAtProp = entity.GetType().GetProperty("CreatedAt");
                var deletedByProp = entity.GetType().GetProperty("DeletedBy");

                if (deletedAtProp != null)
                    deletedAtProp.SetValue(entity, nowUtc);
                if (updatedAtProp != null)
                {
                    var val = updatedAtProp.GetValue(entity) as DateTime?;
                    updatedAtProp.SetValue(entity, DateTime.SpecifyKind(val ?? nowUtc, DateTimeKind.Utc));
                }
                if (createdAtProp != null)
                {
                    var val = createdAtProp.GetValue(entity) as DateTime?;
                    createdAtProp.SetValue(entity, DateTime.SpecifyKind(val ?? nowUtc, DateTimeKind.Utc));
                }
                if (deletedByProp != null)
                    deletedByProp.SetValue(entity, deletedBy);

                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
