using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.Deals
{
    public class Installment : BaseEntity<int>
    {
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public InstallmentStatus Status { get; set; } = InstallmentStatus.Unpaid;
        public DateTime? PaidDate { get; set; }
        public string? ReceiptNumber { get; set; } // رقم الإيصال لو دفع

        // العلاقات
        public int DealId { get; set; }
        public Deal Deal { get; set; } = default!;
    }
}
