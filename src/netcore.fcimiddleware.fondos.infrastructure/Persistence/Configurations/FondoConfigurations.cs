using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.infrastructure.Persistence.Configurations
{
    public class FondoConfigurations : IEntityTypeConfiguration<Fondo>
    {
        public void Configure(EntityTypeBuilder<Fondo> builder)
        {
            builder.ToTable("Fondos");
        }
    }
}
