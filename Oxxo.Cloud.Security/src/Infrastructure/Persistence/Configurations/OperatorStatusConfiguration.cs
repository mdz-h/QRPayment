#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: Administrators.
//===============================================================================
#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    public class OperatorStatusConfiguration : IEntityTypeConfiguration<OperatorStatus>
    {
        public void Configure(EntityTypeBuilder<OperatorStatus> builder)
        {
            builder.HasKey(t => t.OPERATOR_STATUS_ID);
            builder.Property(t => t.OPERATOR_STATUS_ID)
               .IsRequired()
               .HasDefaultValueSql("'NEXT VALUE FOR dbo.OPERATOR_STATUS_SEQ'");

            builder.Property(t => t.CODE)
                .IsRequired(false);

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
