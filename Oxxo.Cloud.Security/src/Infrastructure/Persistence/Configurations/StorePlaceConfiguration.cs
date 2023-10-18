#region File Information
//===============================================================================
// Author:  Omar Toribio Morales Flores (NEORIS).
// Date:    2022-10-06.
// Comment: Entitie configuration store.
//===============================================================================
#endregion
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oxxo.Cloud.Security.Domain.Entities;

namespace Oxxo.Cloud.Security.Infrastructure.Persistence.Configurations;
internal class StorePlaceConfiguration : IEntityTypeConfiguration<StorePlace>
{
    public void Configure(EntityTypeBuilder<StorePlace> builder)
    {
        builder.HasKey(t => t.STORE_PLACE_ID);
        builder.Property(t => t.STORE_PLACE_ID)
            .IsRequired();

        builder.Property(t => t.STORE_ID_SOURCE)
            .IsRequired();

        builder.Property(t => t.PLACE_ID_SOURCE)
            .IsRequired();

        builder.Property(t => t.CR_PLACE)
            .IsRequired();

        builder.Property(t => t.CR_STORE)
           .IsRequired();

        builder.Property(t => t.MODIFIED_BY_OPERATOR_ID)
            .IsRequired(false);

        builder.Property(t => t.MODIFIED_DATETIME)
            .IsRequired(false);

    }
}
