using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.DTOs.Respuesta;
using api_iso_med_pg.Models;
using Microsoft.EntityFrameworkCore;

namespace api_iso_med_pg.Data.Repositories;

public class RespuestaRepository : GenericRepository<Respuesta>, IRespuestaRepository
{
    public RespuestaRepository(AppDbContext context) : base(context) { }


    public void AddRange(IEnumerable<Respuesta> respuestas)
    {
        _context.Set<Respuesta>().AddRange(respuestas);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetEvaluationsDto>> GetTableEvaluationsAsync(int trabajadorId, DateTime dateStart, DateTime dateEnd)
    {
        var query = _context.Set<Respuesta>()
            .Include(r => r.User!)
                .ThenInclude(u => u.Trabajador!)
            .Where(r => r.TrabajadorId == trabajadorId)
            .GroupBy(r => r.NoEvaluacion)
            .Select(g => new
            {
                g.Key,
                AvgType1 = ((g.Where(r => r.Pregunta!.TipoId == 1).Average(r => Convert.ToDouble(r.ValorRespuesta)) - 1) / 3) * 100,
                AvgType2 = ((g.Where(r => r.Pregunta!.TipoId == 2).Average(r => Convert.ToDouble(r.ValorRespuesta)) - 1) / 3) * 100,
                AvgType3 = ((g.Where(r => r.Pregunta!.TipoId == 3).Average(r => Convert.ToDouble(r.ValorRespuesta)) - 1) / 3) * 100,
                AvgType4 = ((g.Where(r => r.Pregunta!.TipoId == 4).Average(r => Convert.ToDouble(r.ValorRespuesta)) - 1) / 3) * 100,
                AvgType5 = ((g.Where(r => r.Pregunta!.TipoId == 5).Average(r => Convert.ToDouble(r.ValorRespuesta)) - 1) / 3) * 100,
                AvgType6 = ((g.Where(r => r.Pregunta!.TipoId == 6).Average(r => Convert.ToDouble(r.ValorRespuesta)) - 1) / 3) * 100,
                Users = g.Select(r => r.User).ToList()
            })
            .ToListAsync();

        var result = (await query).Select(x => new GetEvaluationsDto
        {
            NoEvaluacion = x.Key!,
            AvgType1 = Math.Round(x.AvgType1),
            AvgType2 = Math.Round(x.AvgType2),
            AvgType3 = Math.Round(x.AvgType3),
            AvgType4 = Math.Round(x.AvgType4),
            AvgType5 = Math.Round(x.AvgType5),
            AvgType6 = Math.Round(x.AvgType6),
            Names = x.Users.FirstOrDefault()?.Trabajador?.Nombres ?? ""
        });

        return result.ToList();
    }

    public async Task<GetEvaluationsDto> GetGraphicEvaluationsAsync(int trabajadorId, DateTime dateStart, DateTime dateEnd)
    {
        var respuestas = await _context.Set<Respuesta>()
            .Include(r => r.User!)
                .ThenInclude(u => u.Trabajador!)
            .Include(r => r.Pregunta)
            .Where(r => r.TrabajadorId == trabajadorId)
            .ToListAsync();

        double PromedioTipo(IEnumerable<Respuesta> rs)
            => rs.Any() ? ((rs.Average(r => Convert.ToDouble(r.ValorRespuesta)) - 1) / 3) * 100 : 0;

        var dto = new GetEvaluationsDto
        {
            AvgType1 = Math.Round(PromedioTipo(respuestas.Where(r => r.Pregunta != null && r.Pregunta.TipoId == 1))),
            AvgType2 = Math.Round(PromedioTipo(respuestas.Where(r => r.Pregunta != null && r.Pregunta.TipoId == 2))),
            AvgType3 = Math.Round(PromedioTipo(respuestas.Where(r => r.Pregunta != null && r.Pregunta.TipoId == 3))),
            AvgType4 = Math.Round(PromedioTipo(respuestas.Where(r => r.Pregunta != null && r.Pregunta.TipoId == 4))),
            AvgType5 = Math.Round(PromedioTipo(respuestas.Where(r => r.Pregunta != null && r.Pregunta.TipoId == 5))),
            AvgType6 = Math.Round(PromedioTipo(respuestas.Where(r => r.Pregunta != null && r.Pregunta.TipoId == 6))),
            Names = respuestas.FirstOrDefault()?.User?.Trabajador?.Nombres ?? ""
        };

        return dto;
    }
}