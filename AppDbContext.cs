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
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipamiento>(entity =>
            {
                entity.ToTable("equipamientos");
                // Las propiedades se mapearán automáticamente a snake_case
            });
            // Relación User -> Trabajador
            modelBuilder.Entity<User>()
                .HasOne(u => u.Trabajador)
                .WithMany()
                .HasForeignKey(u => u.TrabajadorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Respuesta -> User (CreadoId)
            modelBuilder.Entity<Respuesta>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.CreadoId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
