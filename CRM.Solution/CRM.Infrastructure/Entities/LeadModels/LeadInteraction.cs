using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.LeadModels
{
    public class LeadInteraction : BaseEntity<int>
    {
        public InteractionType Type { get; set; }
        public DateTime InteractionDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? NextFollowUpDate { get; set; }

        // العلاقات
        public int LeadId { get; set; }
        public Lead Lead { get; set; } = default!;
    }
}
