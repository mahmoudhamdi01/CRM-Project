using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.IdentityModule
{
    public class ApplicationUser : IdentityUser, ISoftDelete
    {
        public string DisplayName { get; set; } = default!;
        public UserRole Role { get; set; }

        // Hierarchy
        public string? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }
        public ICollection<ApplicationUser> Subordinates { get; set; } = new List<ApplicationUser>();

        // Audit & Soft Delete
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
