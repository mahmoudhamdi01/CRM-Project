using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Lead
{
    public class LeadCreateUpdateDTO
    {
        [Required, MaxLength(150)] 
        public string FullName { get; set; } = default!;
        [Required, MaxLength(20)] 
        public string PhoneNumber { get; set; } = default!;
        [EmailAddress] 
        public string? Email { get; set; }
        public LeadStatus Status { get; set; }
        public int SourceId { get; set; }
        public string? AssignedUserId { get; set; }
    }
}
