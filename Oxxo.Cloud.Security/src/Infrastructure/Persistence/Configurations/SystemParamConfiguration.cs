#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie configuration systema param value.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations;
internal class SystemParamConfiguration : IEntityTypeConfiguration<SystemParam>
{
    public void Configure(EntityTypeBuilder<SystemParam> builder)
    {
        builder.HasKey(t => t.SYSTEM_PARAM_ID);

        builder.Property(t => t.SYSTEM_PARAM_ID)
                .IsRequired();

        builder.Property(t => t.SYSTEM_PARAM_ID_SOURCE)
               .IsRequired();

        builder.Property(t => t.SYSTEM_PARAM_VALUE_ID_SOURCE)
               .IsRequired(false);

        builder.Property(t => t.STORE_PLACE_ID)
            .IsRequired(false);

        builder.HasOne(t => t.STORE_PLACE).WithMany(t => t.SYSTEM_PARAM).HasForeignKey(t => t.STORE_PLACE_ID);

        builder.Property(t => t.CR_STORE)
         .IsRequired(false);

        builder.Property(t => t.CR_PLACE)
         .IsRequired(false);

        builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
         .IsRequired(false);

        builder.Property(t => t.MODIFIED_DATETIME)
            .IsRequired(false);
    }
}
