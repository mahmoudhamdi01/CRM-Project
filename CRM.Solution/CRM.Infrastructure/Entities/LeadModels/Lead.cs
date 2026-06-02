using CRM.Infrastructure.Entities.IdentityModule;
using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.LeadModels
{
    public class Lead : BaseEntity<int>
    {
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string? Email { get; set; }
        public LeadStatus Status { get; set; } = LeadStatus.New;

        // العلاقات
        public int SourceId { get; set; }
        public LeadSource Source { get; set; } = default!;

        public string? AssignedUserId { get; set; } // الموظف المسؤول
        public ApplicationUser? AssignedUser { get; set; }

        public ICollection<LeadInteraction> Interactions { get; set; } = new List<LeadInteraction>();
    }
}
