#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie configuration operator.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations;
internal class OperatorConfiguration : IEntityTypeConfiguration<Operator>
{
    public void Configure(EntityTypeBuilder<Operator> builder)
    {
        builder.HasKey(t => t.OPERATOR_ID);
        builder.Property(t => t.OPERATOR_ID)
        .IsRequired();

        builder.Property(t => t.PERSON_ID)
            .IsRequired();

        builder.HasOne(t => t.PERSON)
            .WithOne(t => t.OPERATOR)
            .HasForeignKey<Operator>(b => b.PERSON_ID);

        builder.Property(t => t.OPERATOR_STATUS_ID)
           .IsRequired();

        builder.HasOne(t => t.OPERATOR_STATUS)
            .WithMany(t => t.OPERATORS)
            .HasForeignKey(b => b.OPERATOR_STATUS_ID);

        builder.Property(t => t.FL_INTRN)
         .IsRequired();

        builder.Property(t => t.IS_ACTIVE)
         .IsRequired();

        builder.Property(t => t.LCOUNT)
         .IsRequired();

        builder.Property(t => t.CREATED_BY_OPERATOR_ID)
         .IsRequired();

        builder.Property(t => t.CREATED_DATETIME)
         .IsRequired();

        builder.Property(t => t.USER_NAME)
         .IsRequired(false);

        builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
           .IsRequired(false);

        builder.Property(t => t.MODIFIED_DATETIME)
            .IsRequired(false);
    }
}
