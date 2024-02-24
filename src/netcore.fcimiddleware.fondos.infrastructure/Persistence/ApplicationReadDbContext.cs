using Microsoft.EntityFrameworkCore;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.infrastructure.Persistence
{
    public class ApplicationReadDbContext : DbContext
    {
        public ApplicationReadDbContext(DbContextOptions<ApplicationReadDbContext> options) : base(options)
        {
        }
        
        public DbSet<CondIngEgrFondo>? CondIngEgrFondos { get; set; }
        public DbSet<Fondo>? Fondos { get; set; }
        public DbSet<TpValorCptFondo>? TpValorCptFondos { get; set; }
        public DbSet<AgColocador>? AgColocadores { get; set; }
        public DbSet<Pais>? Paises { get; set; }
        public DbSet<Moneda>? Monedas { get; set; }
        public DbSet<SocGerente>? SocGerentes { get; set; }
        public DbSet<SocDepositaria>? SocDepositarias{ get; set; }
    }
}
