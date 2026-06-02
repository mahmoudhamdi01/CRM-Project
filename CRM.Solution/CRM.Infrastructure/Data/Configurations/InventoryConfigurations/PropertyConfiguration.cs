using CRM.Infrastructure.Entities.RealStateInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Data.Configurations.InventoryConfigurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<PropertyModel>
    {
        public void Configure(EntityTypeBuilder<PropertyModel> builder)
        {
            builder.Property(x => x.UnitCode).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Project)
                   .WithMany(x => x.Properties)
                   .HasForeignKey(x => x.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Owner)
                   .WithMany(x => x.Properties)
                   .HasForeignKey(x => x.OwnerId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
