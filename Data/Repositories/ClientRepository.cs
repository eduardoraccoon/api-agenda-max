using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;

namespace api_iso_med_pg.Data.Repositories
{
    public class ClienteRepository : GenericRepository<Client>, IClienteRepository
    {
        public ClienteRepository(AppDbContext context) : base(context) { }
    }
}
