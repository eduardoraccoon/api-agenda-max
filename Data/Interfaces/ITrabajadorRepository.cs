using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Interfaces;

public interface ITrabajadorRepository
{
    Task<IEnumerable<Trabajador>> GetAllAsync();
    Task<Trabajador?> GetByIdAsync(int id);
}
