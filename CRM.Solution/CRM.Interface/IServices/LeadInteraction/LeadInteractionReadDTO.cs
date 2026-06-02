using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.LeadInteraction
{
    public class LeadInteractionReadDTO
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = default!;
        public DateTime InteractionDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public int LeadId { get; set; }
        public string LeadName { get; set; } = default!;
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
