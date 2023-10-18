#region File Information
//===============================================================================
// Author:  Luis Alberto Caballero Cruz (NEORIS).
// Date:    2022-11-23.
// Comment: Entitie configuration workgroup.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;


namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    internal class WorkgroupConfiguration : IEntityTypeConfiguration<Workgroup>
    {
        public void Configure(EntityTypeBuilder<Workgroup> builder)
        {
            builder.HasKey(t => t.WORKGROUP_ID);
            builder.Property(t => t.WORKGROUP_ID).IsRequired().HasDefaultValueSql("'NEXT VALUE FOR dbo.WORKGROUP_SEQ'");
            builder.Property(t => t.CODE)
            .IsRequired(false);

            builder.Property(t => t.NAME)
            .IsRequired(false);

            builder.Property(t => t.DESCRIPTION)
            .IsRequired(false);

            builder.Property(t => t.IS_ACTIVE);

            builder.Property(t => t.LCOUNT)
            .IsRequired();

            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
            .IsRequired(false);

            builder.Property(t => t.MODIFIED_DATETIME)
            .IsRequired(false);
        }
    }
}
