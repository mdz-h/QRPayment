using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    public class DeviceTypeConfiguration : IEntityTypeConfiguration<DeviceType>
    {
        public void Configure(EntityTypeBuilder<DeviceType> builder)
        {
            builder.HasKey(t => t.DEVICE_TYPE_ID);

            builder.Property(t => t.DEVICE_TYPE_ID)
                .IsRequired();

            builder.Property(t => t.CODE)
                .IsRequired();

            builder.Property(t => t.NAME)
               .IsRequired();

            builder.Property(t => t.DESCRIPTION)
                .IsRequired();


            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
               .IsRequired(false);

            builder.Property(t => t.MODIFIED_DATETIME)
               .IsRequired(false);
        }
    }
}
