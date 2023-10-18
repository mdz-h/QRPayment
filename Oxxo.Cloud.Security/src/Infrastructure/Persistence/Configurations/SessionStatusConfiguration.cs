#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie configuration session status.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations;
internal class SessionStatusConfiguration : IEntityTypeConfiguration<SessionStatus>
{
    public void Configure(EntityTypeBuilder<SessionStatus> builder)
    {
        builder.HasKey(t => t.SESSION_STATUS_ID);

        builder.Property(t => t.SESSION_STATUS_ID)
            .IsRequired();

        builder.Property(t => t.CODE)
           .IsRequired(false);

        builder.Property(t => t.NAME)
           .IsRequired(false);

        builder.Property(t => t.DESCRIPTION)
           .IsRequired(false);

        builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
           .IsRequired(false);

        builder.Property(t => t.MODIFIED_DATETIME)
           .IsRequired(false);
    }
}
