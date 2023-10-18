using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    internal class ValidIPRangeConfiguration : IEntityTypeConfiguration<ValidIPRange>
    {
        public void Configure(EntityTypeBuilder<ValidIPRange> builder)
        {
            builder.HasKey(t => t.VALID_IP_RANGE_IP);

            builder.Property(t => t.VALID_IP_RANGE_IP)
                .IsRequired();
            builder.Property(e => e.VALID_IP_RANGE_IP).HasDefaultValueSql("'NEXT VALUE FOR dbo.VALID_IP_RANGE_SEQ'");

            builder.Property(t => t.IP_RANGE)
               .IsRequired(false);

            builder.Property(t => t.DESCRIPTION)
               .IsRequired(false);

            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
               .IsRequired(false);

            builder.Property(t => t.MODIFIED_DATETIME)
               .IsRequired(false);
        }
    }
}
