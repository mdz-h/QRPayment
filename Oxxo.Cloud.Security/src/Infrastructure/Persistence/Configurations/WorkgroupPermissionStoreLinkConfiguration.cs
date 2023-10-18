#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-08.
// Comment: Entity WorkgroupPermissionStoreLink
//===============================================================================
#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    public class WorkgroupPermissionStoreLinkConfiguration : IEntityTypeConfiguration<WorkgroupPermissionStoreLink>
    {
        public void Configure(EntityTypeBuilder<WorkgroupPermissionStoreLink> builder)
        {
            builder.HasKey(t => t.WORKGROUP_PERMISSION_STORE_LINK_ID);
            builder.Property(t => t.WORKGROUP_PERMISSION_LINK_ID)
               .IsRequired();

            builder.Property(t => t.WORKGROUP_PERMISSION_LINK_ID)
               .IsRequired();

            builder.Property(t => t.STORE_PLACE_ID)
               .IsRequired();

            builder.Property(t => t.IS_ACTIVE)
                .IsRequired();

            builder.Property(t => t.LCOUNT)
                .IsRequired();

            builder.Property(t => t.CREATED_BY_OPERATOR_ID)
                .IsRequired();

            builder.Property(t => t.CREATED_DATETIME)
                .IsRequired();

            builder.Property(t => t.MODIFIED_DATETIME)
                .IsRequired(false);

            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
                .IsRequired();
        }
    }
}
