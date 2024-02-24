using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.infrastructure.Persistence.Configurations
{
    public class CondIngEgrFondoConfigurations : IEntityTypeConfiguration<CondIngEgrFondo>
    {
        public void Configure(EntityTypeBuilder<CondIngEgrFondo> builder)
        {
            builder.ToTable("CondIngEgrFondos");
        }
    }
}
