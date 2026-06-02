using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Deal
{
    public class DealReadDTO
    {
        public int Id { get; set; }
        public DateTime DealDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string StatusName { get; set; } = default!;
        public string? Notes { get; set; }
        public string LeadName { get; set; } = default!;
        public string PropertyCode { get; set; } = default!;
        public string? AssignedUserName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
