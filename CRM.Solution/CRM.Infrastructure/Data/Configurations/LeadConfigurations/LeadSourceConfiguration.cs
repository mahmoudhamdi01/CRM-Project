using CRM.Infrastructure.Entities.LeadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Data.Configurations.LeadConfigurations
{
    public class LeadSourceConfiguration : IEntityTypeConfiguration<LeadSource>
    {
        public void Configure(EntityTypeBuilder<LeadSource> builder)
        {
            builder.Property(x => x.TitleArabic).IsRequired().HasMaxLength(100);
            builder.Property(x => x.TitleEnglish).IsRequired().HasMaxLength(100);
        }
    }
}
