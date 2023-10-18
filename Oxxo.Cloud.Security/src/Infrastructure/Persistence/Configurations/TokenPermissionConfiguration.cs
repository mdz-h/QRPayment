using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Common.Models;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    internal class TokenPermissionConfiguration : IEntityTypeConfiguration<TokenPermission>
    {
        public void Configure(EntityTypeBuilder<TokenPermission> builder)
        {
            builder.HasNoKey();
        }
    }

}
