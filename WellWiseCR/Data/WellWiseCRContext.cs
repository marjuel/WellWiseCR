using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WellWiseCR.Models;

namespace WellWiseCR.Data
{
    public class WellWiseCRContext : DbContext
    {
        public WellWiseCRContext(DbContextOptions<WellWiseCRContext> options)
            : base(options)
        {
        }

        public DbSet<WellWiseCR.Models.Usuario> Usuario { get; set; } = default!;

        public DbSet<WellWiseCR.Models.Especialidad>? Especialidad { get; set; }

        public DbSet<WellWiseCR.Models.Especialista>? Especialista { get; set; }

        public DbSet<WellWiseCR.Models.Enfermedad>? Enfermedad { get; set; }

        public DbSet<WellWiseCR.Models.Diagnostico>? Diagnostico { get; set; }

        public DbSet<WellWiseCR.Models.Detalle>? Detalle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Detalle>()
                .HasKey(x => new { x.IdDiagnostico, x.IdEnfermedad });

            modelBuilder.Entity<Detalle>()
            .HasOne(d => d.Diagnostico)
            .WithMany(d => d.Detalle)
            .HasForeignKey(d => d.IdDiagnostico);

            modelBuilder.Entity<Detalle>()
                .HasOne(d => d.Enfermedad)
                .WithMany(e => e.Detalle)
                .HasForeignKey(d => d.IdEnfermedad);
        }
    }
}
