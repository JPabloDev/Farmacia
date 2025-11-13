using FarmaciaApi.Models.Entities;
using LibraryApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data
{
    public class FarmaciaDbContext : DbContext
    {
        public FarmaciaDbContext(DbContextOptions<FarmaciaDbContext> options) : base(options)
        { }
        public DbSet<Users> Usuarios { get; set; }
        public DbSet<Medicamentos> Medicamento { get; set; }

        public DbSet<Ventas> Venta{ get; set; }

        public DbSet<VentaDetalle> VentaDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Medicamento ---
            modelBuilder.Entity<Medicamentos>(entity =>
            {
                entity.ToTable("Medicamento");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PrecioCompra).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PrecioVenta).HasColumnType("decimal(18,2)");
            });

            // --- Venta ---
            modelBuilder.Entity<Ventas>(entity =>
            {
                entity.ToTable("Venta");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
                entity.HasMany(e => e.Detalles)
                      .WithOne(d => d.Venta)
                      .HasForeignKey(d => d.VentaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- VentaDetalle ---
            modelBuilder.Entity<VentaDetalle>(entity =>
            {
                entity.ToTable("VentaDetalle");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.Medicamento)
                      .WithMany(m => m.VentaDetalles)
                      .HasForeignKey(d => d.MedicamentoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        
    }
    }
}