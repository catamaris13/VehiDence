using Microsoft.EntityFrameworkCore;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Data
{
    public class AplicatieDbContext : DbContext
    {
        public AplicatieDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Asigurare> Asigurare { get; set; }
        public DbSet<Casco> Casco { get; set; }
        public DbSet<ITP> ITP { get; set; }
        public DbSet<Masina> Masina { get; set; }
        public DbSet<PermisConducere> PermisConducere { get; set; }
        public DbSet<RevizieService> RevizieService { get; set; }
        public DbSet<Vigneta> Vigneta { get; set; }
    }
}
