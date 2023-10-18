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
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(t => t.PERMISSION_ID);
            builder.Property(t => t.PERMISSION_ID)
               .IsRequired()
               .HasDefaultValueSql("'NEXT VALUE FOR dbo.PERMISSION_SEQ'");

            builder.Property(t => t.CODE)
                .IsRequired(false);

            builder.Property(t => t.DESCRIPTION)
                .IsRequired(false);

            builder.Property(t => t.MODIFIED_DATETIME)
           .IsRequired(false);            
        }
    }
}
