using BackendCore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCore.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Grupo> Grupos => Set<Grupo>();
        public DbSet<Miembro> Miembros => Set<Miembro>();
        public DbSet<Gasto> Gastos => Set<Gasto>();
        public DbSet<Saldo> Saldos => Set<Saldo>();
        public DbSet<Deuda> Deudas => Set<Deuda>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.PagadoPor)
                .WithMany(m => m.GastosPagados)
                .HasForeignKey(g => g.PagadoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Saldo>()
                .HasOne(s => s.Deudor)
                .WithMany()
                .HasForeignKey(s => s.DeudorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Saldo>()
                .HasOne(s => s.Acreedor)
                .WithMany()
                .HasForeignKey(s => s.AcreedorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Deuda>()
                .HasOne(d => d.Deudor)
                .WithMany()
                .HasForeignKey(d => d.DeudorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Deuda>()
                .HasOne(d => d.Acreedor)
                .WithMany()
                .HasForeignKey(d => d.AcreedorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Gasto>()
                .Property(g => g.Monto)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Saldo>()
                .Property(s => s.Monto)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Deuda>()
                .Property(d => d.Monto)
                .HasColumnType("decimal(18,2)");
        }
    }
}
