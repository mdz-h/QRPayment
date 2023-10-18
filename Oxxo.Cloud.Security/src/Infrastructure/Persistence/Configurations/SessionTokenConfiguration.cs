#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie configuration session token.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations;
internal class SessionTokenConfiguration : IEntityTypeConfiguration<SessionToken>
{
    public void Configure(EntityTypeBuilder<SessionToken> builder)
    {
        builder.HasKey(t => t.SESSION_TOKEN_ID);
        builder.Property(t => t.SESSION_TOKEN_ID)
              .IsRequired();
        builder.Property(e => e.SESSION_TOKEN_ID).HasDefaultValueSql("'NEXT VALUE FOR dbo.SESSION_TOKEN_SEQ'");

        builder.Property(t => t.REFRESH_TOKEN)
            .IsRequired(false);

        builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
            .IsRequired(false);

        builder.Property(t => t.MODIFIED_DATETIME)
            .IsRequired(false);
    }
}
