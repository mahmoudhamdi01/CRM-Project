using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Lead
{
    public class LeadQueryParams : BaseQueryParams
    {
        public LeadStatus? Status { get; set; }
        public string? AssignedUserId { get; set; }
    }
}
