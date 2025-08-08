using api_iso_med_pg.Models;
using Microsoft.EntityFrameworkCore;

namespace api_iso_med_pg
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Compania> Companias { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Norma> Normas { get; set; }
        public DbSet<Scrum> Scrums { get; set; }
        public DbSet<Entrevista> Entrevistas { get; set; }
        public DbSet<Equipamiento> Equipamientos { get; set; }
        public DbSet<Trabajador> Trabajadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración específica para Equipamiento si necesitas personalizar algo
            modelBuilder.Entity<Equipamiento>(entity =>
            {
                entity.ToTable("equipamientos");
                // Las propiedades se mapearán automáticamente a snake_case
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
