using CRM.Infrastructure.Entities.Deals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Data.Configurations.DealConfiguration
{
    public class DealConfiguration : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Notes).HasMaxLength(1000);

            builder.HasOne(x => x.Lead)
                   .WithMany()
                   .HasForeignKey(x => x.LeadId)
                   .OnDelete(DeleteBehavior.Restrict); // منع مسح العميل لو ليه صفقات

            builder.HasOne(x => x.Property)
                   .WithMany()
                   .HasForeignKey(x => x.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict); // منع مسح الوحدة لو مباعة

            builder.HasOne(x => x.AssignedUser)
                   .WithMany()
                   .HasForeignKey(x => x.AssignedUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
