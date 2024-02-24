using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.infrastructure.Persistence.Configurations
{
    public class SocGerenteConfigurations : IEntityTypeConfiguration<SocGerente>
    {
        public void Configure(EntityTypeBuilder<SocGerente> builder)
        {
            builder.ToTable("SocGerentes");
        }
    }
}
