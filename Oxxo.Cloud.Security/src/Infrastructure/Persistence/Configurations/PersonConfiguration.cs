#region File Information
//===============================================================================
// Author:  FREDEL REYNEL PACHECO CAAMAL (NEORIS).
// Date:    06/12/2022.
// Comment: Administrators.
//===============================================================================
#endregion


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(t => t.PERSON_ID);
            builder.Property(t => t.PERSON_ID)
                .IsRequired()
                .HasDefaultValueSql("'NEXT VALUE FOR dbo.PERSON_SEQ'");

            builder.Property(t => t.NAME)
                .IsRequired();

            builder.Property(t => t.MIDDLE_NAME)
                .IsRequired(false);

            builder.Property(t => t.LASTNAME_PAT)
                .IsRequired();

            builder.Property(t => t.LASTNAME_MAT)
                .IsRequired();

            builder.Property(t => t.EMAIL)
                .IsRequired(false);

            builder.Property(t => t.IS_ACTIVE)
                .IsRequired();

            builder.Property(t => t.LCOUNT)
                .IsRequired();

            builder.Property(t => t.CREATED_BY_OPERATOR_ID)
                .IsRequired();

            builder.Property(t => t.CREATED_DATETIME)
                .IsRequired();

            builder.Property(t => t.MODIFIED_DATETIME)
                .IsRequired(false);

            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
                .IsRequired(false);
        }
    }
}
