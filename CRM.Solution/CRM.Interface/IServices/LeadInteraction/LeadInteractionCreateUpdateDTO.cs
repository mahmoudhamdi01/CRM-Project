using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.LeadInteraction
{
    public class LeadInteractionCreateUpdateDTO
    {
        public InteractionType Type { get; set; }
        public DateTime InteractionDate { get; set; }
        [MaxLength(1000)] 
        public string? Notes { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public int LeadId { get; set; }
    }
}
