using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories;

public class PreguntaRepository : GenericRepository<Pregunta>, IPreguntaRepository
{
    public PreguntaRepository(AppDbContext context) : base(context) { }
}
