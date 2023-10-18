#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie configuration external application.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations;
public class ExternalApplicationConfigurations : IEntityTypeConfiguration<ExternalApplication>
{
    public void Configure(EntityTypeBuilder<ExternalApplication> builder)
    {
        builder.HasKey(t => t.EXTERNAL_APPLICATION_ID);
        builder.Property(t => t.EXTERNAL_APPLICATION_ID)
            .IsRequired();

        builder.Property(t => t.CODE)
            .IsRequired(false);

        builder.Property(t => t.NAME)
            .IsRequired(false);

        builder.Property(t => t.TIME_EXPIRATION_TOKEN)
                .IsRequired();

        builder.Property(t => t.IS_ACTIVE)
               .IsRequired();

        builder.Property(t => t.LCOUNT)
            .IsRequired();

        builder.Property(t => t.CREATED_BY_OPERATOR_ID)
            .IsRequired();

        builder.Property(t => t.CREATED_DATETIME)
            .IsRequired();

        builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
            .IsRequired(false);

        builder.Property(t => t.MODIFIED_DATETIME)
            .IsRequired(false);
    }
}
