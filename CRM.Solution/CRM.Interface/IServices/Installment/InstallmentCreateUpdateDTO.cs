using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Installment
{
    public class InstallmentCreateUpdateDTO
    {
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public InstallmentStatus Status { get; set; }
        public DateTime? PaidDate { get; set; }
        [MaxLength(50)] public string? ReceiptNumber { get; set; }
        public int DealId { get; set; }
    }
}
