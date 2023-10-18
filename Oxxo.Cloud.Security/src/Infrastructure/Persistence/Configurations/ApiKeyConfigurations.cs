#region File Information
//===============================================================================
// Author:  Fredel Reynel Pacheco Caamal (NEORIS).
// Date:    2022-12-21.
// Comment: GENERATE API KEY.
//===============================================================================
#endregion


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Configurations
    /// </summary>
    public class ApiKeyConfigurations : IEntityTypeConfiguration<ApiKey>
    {
        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Configure(EntityTypeBuilder<ApiKey> builder)
        {
            builder.HasKey(t => t.API_KEY_ID);
            builder.Property(t => t.API_KEY_ID)
                .IsRequired()
                .HasDefaultValueSql("'NEXT VALUE FOR API_KEY_SEQ'");

            builder.Property(t => t.EXTERNAL_APPLICATION_ID)
                .IsRequired();

            builder.HasOne(t => t.EXTERNAL_APPLICATION)
                .WithMany(t => t.APIKEYS)
                .HasForeignKey(t => t.EXTERNAL_APPLICATION_ID);

            builder.Property(t => t.API_KEY)
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
