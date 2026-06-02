using CRM.Infrastructure.Entities.IdentityModule;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Infrastructure.Entities.RealStateInventory;
using CRM.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.Deals
{
    public class Deal : BaseEntity<int>
    {
        public DateTime DealDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DealStatus Status { get; set; } = DealStatus.Pending;
        public string? Notes { get; set; }

        // العلاقات
        public int LeadId { get; set; }
        public Lead Lead { get; set; } = default!;

        public int PropertyId { get; set; }
        public PropertyModel Property { get; set; } = default!;

        public string? AssignedUserId { get; set; } // موظف المبيعات اللي قفل الديل
        public ApplicationUser? AssignedUser { get; set; }

        public ICollection<Installment> Installments { get; set; } = new List<Installment>();
    }
}
