using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Repositories;

public class EquipamientoRepository : GenericRepository<Equipamiento>, IEquipamientoRepository
{
    public EquipamientoRepository(AppDbContext context) : base(context) { }
}
