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
        public WellWiseCRContext (DbContextOptions<WellWiseCRContext> options)
            : base(options)
        {
        }

        public DbSet<WellWiseCR.Models.Usuario> Usuario { get; set; } = default!;

        public DbSet<WellWiseCR.Models.Especialidad>? Especialidad { get; set; }
    }
}
