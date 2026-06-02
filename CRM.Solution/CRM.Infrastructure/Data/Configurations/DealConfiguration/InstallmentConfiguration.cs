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
    public class InstallmentConfiguration : IEntityTypeConfiguration<Installment>
    {
        public void Configure(EntityTypeBuilder<Installment> builder)
        {
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.ReceiptNumber).HasMaxLength(50);

            builder.HasOne(x => x.Deal)
                   .WithMany(x => x.Installments)
                   .HasForeignKey(x => x.DealId)
                   .OnDelete(DeleteBehavior.Cascade); // لو الصفقة اتمسحت، الأقساط تتمسح
        }
    }
}
