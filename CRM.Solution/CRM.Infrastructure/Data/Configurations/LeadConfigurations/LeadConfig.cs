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
    public class LeadConfig : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);

            builder.HasOne(x => x.Source)
                   .WithMany(x => x.Leads)
                   .HasForeignKey(x => x.SourceId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssignedUser)
                   .WithMany()
                   .HasForeignKey(x => x.AssignedUserId)
                   .OnDelete(DeleteBehavior.SetNull); // لو الموظف اتمسح، العميل يفضل موجود بدون مسؤول
        }
    }
}
