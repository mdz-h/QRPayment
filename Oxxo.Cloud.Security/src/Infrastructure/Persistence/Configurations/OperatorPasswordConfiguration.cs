using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    public class OperatorPasswordConfiguration : IEntityTypeConfiguration<OperatorPassword>
    {
        public void Configure(EntityTypeBuilder<OperatorPassword> builder)
        {
            builder.HasKey(t => t.OPERATOR_PASSWORD_ID);

            builder.Property(t => t.OPERATOR_PASSWORD_ID)
            .IsRequired();

            builder.Property(e => e.OPERATOR_PASSWORD_ID)
                .HasDefaultValueSql("'NEXT VALUE FOR dbo.OPERATOR_PASSWORD_SEQ'");

            builder.Property(t => t.OPERATOR_ID)
            .IsRequired();

            builder.HasOne(d => d.OPERATOR)
                    .WithMany(p => p.OPERATOR_PASSWORD)
                    .HasForeignKey(d => d.OPERATOR_ID);

            builder.Property(t => t.PASSWORD)
               .IsRequired();

            builder.Property(t => t.EXPIRATION_TIME)
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
}
