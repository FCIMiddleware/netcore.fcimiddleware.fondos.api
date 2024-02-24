using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.infrastructure.Persistence.Configurations
{
    public class TpValorCptFondoConfigurations : IEntityTypeConfiguration<TpValorCptFondo>
    {
        public void Configure(EntityTypeBuilder<TpValorCptFondo> builder)
        {
            builder.ToTable("TpValorCptFondos");
        }
    }
}
