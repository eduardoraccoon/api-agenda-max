using api_iso_med_pg.Models;
using Microsoft.EntityFrameworkCore;

namespace api_iso_med_pg
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Norma> Normas { get; set; }
        public DbSet<Scrum> Scrums { get; set; }
        public DbSet<Equipamiento> Equipamientos { get; set; }
    }
}
