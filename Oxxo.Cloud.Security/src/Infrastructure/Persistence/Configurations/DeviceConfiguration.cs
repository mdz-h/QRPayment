#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie configuration device.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations;
internal class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(t => t.DEVICE_ID);

        builder.Property(t => t.DEVICE_ID)
            .IsRequired();
        builder.Property(e => e.DEVICE_ID).HasDefaultValueSql("'DEFAULT (newsequentialid())'");

        builder.Property(t => t.STORE_PLACE_ID)
            .IsRequired();
        builder.HasOne(t => t.STORE_PLACE).WithMany(t => t.DEVICES).HasForeignKey(t => t.STORE_PLACE_ID);

        builder.Property(t => t.DEVICE_NUMBER_ID)
           .IsRequired();

        builder.HasOne(t => t.DEVICE_NUMBER).WithMany(t => t.DEVICES).HasForeignKey(t => t.DEVICE_NUMBER_ID);

        builder.Property(t => t.DEVICE_TYPE_ID)
            .IsRequired();

        builder.HasOne(t => t.DEVICE_TYPE).WithMany(t => t.DEVICES).HasForeignKey(t => t.DEVICE_TYPE_ID);

        builder.Property(t => t.DEVICE_STATUS_ID)
           .IsRequired();
        builder.HasOne(t => t.DEVICE_STATUS).WithMany(t => t.DEVICES).HasForeignKey(t => t.DEVICE_STATUS_ID);

        builder.Property(t => t.MAC_ADDRESS)
           .IsRequired(false);

        builder.Property(t => t.IP)
           .IsRequired(false);

        builder.Property(t => t.PROCESSOR)
           .IsRequired(false);

        builder.Property(t => t.NETWORK_CARD)
           .IsRequired(false);

        builder.Property(t => t.NAME)
           .IsRequired(false);

        builder.Property(t => t.DESCRIPTION)
           .IsRequired(false);

        builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
           .IsRequired(false);

        builder.Property(t => t.MODIFIED_DATETIME)
           .IsRequired(false);
    }
}
