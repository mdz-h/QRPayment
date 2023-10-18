#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Operator store link configuration.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    public class OperatorStoreLinkConfiguration : IEntityTypeConfiguration<OperatorStoreLink>
    {
        public void Configure(EntityTypeBuilder<OperatorStoreLink> builder)
        {
            builder.HasKey(t => t.OPERATOR_STORE_LINK_ID);

            builder.Property(t => t.OPERATOR_STORE_LINK_ID)
                .IsRequired();
            builder.Property(e => e.OPERATOR_STORE_LINK_ID).HasDefaultValueSql("'DEFAULT (newsequentialid())'");

            builder.Property(t => t.STORE_PLACE_ID)
                .IsRequired();
            builder.HasOne(t => t.STOREPLACE).WithMany(t => t.OPERATORSTORELINK).HasForeignKey(t => t.STORE_PLACE_ID);

            builder.Property(t => t.IS_ACTIVE)
                .IsRequired();

            builder.Property(t => t.LCOUNT)
                .IsRequired();

            builder.Property(t => t.CREATED_DATETIME)
               .IsRequired();

            builder.Property(t => t.CREATED_BY_OPERATOR_ID)
             .IsRequired();

            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
               .IsRequired(false);

            builder.Property(t => t.MODIFIED_DATETIME)
               .IsRequired(false);

            builder.Property(t => t.SESSION_TOKEN_ID)
               .IsRequired();
            builder.HasOne(t => t.SESSIONTOKEN).WithMany(t => t.OPERATORSTORELINKS).HasForeignKey(t => t.SESSION_TOKEN_ID);
        }
    }
}
