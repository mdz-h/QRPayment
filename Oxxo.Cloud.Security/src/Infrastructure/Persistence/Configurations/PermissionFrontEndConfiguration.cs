#region File Information
//===============================================================================
// Author:  Alfonso Zavala Beristáin (NEORIS).
// Date:    08/05/2023.
// Comment: Class PermissionFrontEndConfiguration
//===============================================================================
#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    public class PermissionFrontEndConfiguration : IEntityTypeConfiguration<PermissionFrontEnd>
    {
        public void Configure(EntityTypeBuilder<PermissionFrontEnd> builder)
        {
            builder.HasKey(t => t.PERMISSION_FRONTEND_ID);
            builder.Property(t => t.PERMISSION_FRONTEND_ID)
               .IsRequired()
               .HasDefaultValueSql("'NEXT VALUE FOR PERMISSION_FRONTEND_SEQ'");

            builder.Property(t => t.PERMISSION_ID)
               .IsRequired();
            builder.HasOne(t => t.PERMISSIONS).WithMany(t => t.PERMISSIONFRONTEND)
                .HasForeignKey(t => t.PERMISSION_ID);
        }
    }
}