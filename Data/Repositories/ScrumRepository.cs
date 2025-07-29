using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_iso_med_pg.Data.Repositories
{
    public class ScrumRepository : GenericRepository<Scrum>, IScrumRepository
    {
        public ScrumRepository(AppDbContext context) : base(context)
        {
        }
    }
}
