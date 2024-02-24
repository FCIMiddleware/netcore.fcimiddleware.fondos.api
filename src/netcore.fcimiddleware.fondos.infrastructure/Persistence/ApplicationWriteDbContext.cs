using Microsoft.EntityFrameworkCore;
using netcore.fcimiddleware.fondos.domain.Common;
using netcore.fcimiddleware.fondos.domain;
using System.Reflection;

namespace netcore.fcimiddleware.fondos.infrastructure.Persistence
{
    public class ApplicationWriteDbContext : DbContext
    {
        public ApplicationWriteDbContext(DbContextOptions<ApplicationWriteDbContext> options) : base(options)
        {
        }

        public DbSet<CondIngEgrFondo>? CondIngEgrFondos { get; set; }
        public DbSet<Fondo>? Fondos { get; set; }
        public DbSet<TpValorCptFondo>? TpValorCptFondos { get; set; }
        public DbSet<AgColocador>? AgColocadores { get; set; }
        public DbSet<Pais>? Paises { get; set; }
        public DbSet<Moneda>? Monedas { get; set; }
        public DbSet<SocGerente>? SocGerentes { get; set; }
        public DbSet<SocDepositaria>? SocDepositarias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "system";
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "system";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
