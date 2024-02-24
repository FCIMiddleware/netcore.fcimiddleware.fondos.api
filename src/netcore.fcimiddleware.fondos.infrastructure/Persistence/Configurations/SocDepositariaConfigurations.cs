using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.infrastructure.Persistence.Configurations
{
    public class SocDepositariaConfigurations : IEntityTypeConfiguration<SocDepositaria>
    {
        public void Configure(EntityTypeBuilder<SocDepositaria> builder)
        {
            builder.ToTable("SocDepositarias");
        }
    }
}
