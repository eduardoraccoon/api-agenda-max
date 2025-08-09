using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_iso_med_pg.Data.Repositories;

public class TrabajadorRepository : ITrabajadorRepository
{
    private readonly AppDbContext _context;

    public TrabajadorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trabajador>> GetAllAsync()
    {
        return await _context.Trabajadores.Where(t => t.Estatus == 1).ToListAsync();
    }

    public async Task<Trabajador?> GetByIdAsync(int id)
    {
        return await _context.Trabajadores.FindAsync(id);
    }
}
