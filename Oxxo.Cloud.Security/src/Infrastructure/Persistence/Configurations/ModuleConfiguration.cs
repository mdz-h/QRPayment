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
    internal class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(t => t.MODULE_ID);
            builder.Property(t => t.MODULE_ID)
               .IsRequired();

            builder.Property(t => t.APPLICATION_ID)
               .IsRequired();

            builder.Property(t => t.NAME)
                .IsRequired(false);

            builder.Property(t => t.DESCRIPTION)
                .IsRequired(false);


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
                .IsRequired(false);
        }
    }
}
