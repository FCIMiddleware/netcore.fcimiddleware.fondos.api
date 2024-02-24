using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.infrastructure.Persistence.Configurations
{
    public class AgColocadorConfigurations : IEntityTypeConfiguration<AgColocador>
    {
        public void Configure(EntityTypeBuilder<AgColocador> builder)
        {
            builder.ToTable("AgColocadores");
        }
    }
}
