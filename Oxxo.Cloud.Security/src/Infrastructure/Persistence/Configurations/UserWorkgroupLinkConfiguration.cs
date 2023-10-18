using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations
{
    internal class UserWorkgroupLinkConfiguration : IEntityTypeConfiguration<UserWorkgroupLink>
    {
        public void Configure(EntityTypeBuilder<UserWorkgroupLink> builder)
        {
            builder.HasKey(t => t.USER_WORKGROUP_LINK_ID);
            builder.Property(t => t.USER_WORKGROUP_LINK_ID).IsRequired().HasDefaultValueSql("'NEXT VALUE FOR dbo.USER_WORKGROUP_LINK_SEQ'");
            builder.Property(t => t.WORKGROUP_ID).IsRequired();
            builder.HasOne(t => t.WORKGROUP).WithMany(t => t.USER_WORKGROUP_LINKS).HasForeignKey(t => t.WORKGROUP_ID);
            builder.Property(t => t.GUID).IsRequired();
            builder.Property(t => t.IS_ACTIVE).IsRequired();
            builder.Property(t => t.LCOUNT).IsRequired();
            builder.Property(t => t.CREATED_BY_OPERATOR_ID).IsRequired();
            builder.Property(t => t.CREATED_DATETIME).IsRequired();
            builder.Property(t => t.MODIFIED_BY_OPERATOR_ID).IsRequired(false);
            builder.Property(t => t.MODIFIED_DATETIME).IsRequired(false);
        }
    }
}
