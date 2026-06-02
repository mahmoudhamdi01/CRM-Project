using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Installment
{
    public class InstallmentReadDTO
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string StatusName { get; set; } = default!;
        public DateTime? PaidDate { get; set; }
        public string? ReceiptNumber { get; set; }
        public int DealId { get; set; }
    }
}
