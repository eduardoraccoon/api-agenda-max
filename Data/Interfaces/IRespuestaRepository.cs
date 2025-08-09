using api_iso_med_pg.DTOs.Respuesta;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Interfaces
{
    public interface IRespuestaRepository : IGenericRepository<Respuesta>
    {
        Task<IEnumerable<GetEvaluationsDto>> GetTableEvaluationsAsync(int trabajadorId, DateTime dateStart, DateTime dateEnd);
    }
}
    
