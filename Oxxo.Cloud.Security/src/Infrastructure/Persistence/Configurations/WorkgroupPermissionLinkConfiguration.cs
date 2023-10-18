
#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    28/11/2022.
// Comment: permissions.
//===============================================================================
#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    public class WorkgroupPermissionLinkConfiguration : IEntityTypeConfiguration<WorkgroupPermissionLink>
    {
        public void Configure(EntityTypeBuilder<WorkgroupPermissionLink> builder)
        {
            builder.HasKey(t => t.WORKGROUP_PERMISSION_LINK_ID);
            builder.Property(t => t.WORKGROUP_PERMISSION_LINK_ID)
               .IsRequired()
               .HasDefaultValueSql("'NEXT VALUE FOR dbo.WORKGROUP_PERMISSION_LINK_SEQ'");
            builder.Property(t => t.WORKGROUP_ID)
               .IsRequired();

            builder.Property(t => t.PERMISSION_ID)
               .IsRequired();

            builder.Property(t => t.IS_ACTIVE)
                .IsRequired();

            builder.Property(t => t.LCOUNT)
                .IsRequired();

            builder.Property(t => t.CREATED_BY_OPERATOR_ID)
                .IsRequired();

            builder.Property(t => t.CREATED_DATETIME)
                .IsRequired();

            builder.Property(t => t.WORKGROUP_ID)
           .IsRequired();

            builder.HasOne(t => t.WORKGROUP).WithMany(t => t.WORKGROUP).HasForeignKey(t => t.WORKGROUP_ID);

            builder.Property(t => t.PERMISSION_ID)
            .IsRequired();
            builder.HasOne(t => t.PERMISSION).WithMany(t => t.WORKGROUPPERMISSIONLINK).HasForeignKey(t => t.PERMISSION_ID);

            builder.Property(t => t.MODIFIED_DATETIME)
                .IsRequired(false);

            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
                .IsRequired(false);
        }
    }
}
