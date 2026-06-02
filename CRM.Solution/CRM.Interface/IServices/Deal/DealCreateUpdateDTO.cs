using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Deal
{
    public class DealCreateUpdateDTO
    {
        public DateTime DealDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DealStatus Status { get; set; }
        [MaxLength(1000)] public string? Notes { get; set; }
        public int LeadId { get; set; }
        public int PropertyId { get; set; }
        public string? AssignedUserId { get; set; }
    }
}
