using CRM.Infrastructure.Entities.RealStateInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Data.Configurations.InventoryConfigurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(x => x.TitleArabic).IsRequired().HasMaxLength(100);
            builder.Property(x => x.TitleEnglish).IsRequired().HasMaxLength(100);
            builder.Property(x => x.DeveloperName).HasMaxLength(100);
            builder.Property(x => x.Location).HasMaxLength(200);
        }
    }
}
