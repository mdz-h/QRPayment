using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    internal class DeviceStatusConfiguration : IEntityTypeConfiguration<DeviceStatus>
    {
        public void Configure(EntityTypeBuilder<DeviceStatus> builder)
        {
            builder.HasKey(t => t.DEVICE_STATUS_ID);

            builder.Property(t => t.DEVICE_STATUS_ID)
                .IsRequired();

            builder.Property(t => t.CODE)
               .IsRequired();

            builder.Property(t => t.NAME)
                .IsRequired(false);

            builder.Property(t => t.DESCRIPTION)
               .IsRequired(false);

            builder.Property(t => t.IS_ACTIVE)
                .IsRequired();

            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
            .IsRequired(false);

            builder.Property(t => t.MODIFIED_DATETIME)
               .IsRequired(false);
        }
    }
}
