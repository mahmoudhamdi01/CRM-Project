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
    public class LeadInteractionConfiguration : IEntityTypeConfiguration<LeadInteraction>
    {
        public void Configure(EntityTypeBuilder<LeadInteraction> builder)
        {
            builder.Property(x => x.Notes).HasMaxLength(1000);

            builder.HasOne(x => x.Lead)
                   .WithMany(x => x.Interactions)
                   .HasForeignKey(x => x.LeadId)
                   .OnDelete(DeleteBehavior.Cascade); // لو العميل اتمسح، تاريخ متابعاته يتمسح
        }
    }
}
