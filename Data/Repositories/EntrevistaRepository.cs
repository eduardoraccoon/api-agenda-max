using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Repositories;

public class EntrevistaRepository : GenericRepository<Entrevista>, IEntrevistaRepository
{
    public EntrevistaRepository(AppDbContext context) : base(context) { }
}