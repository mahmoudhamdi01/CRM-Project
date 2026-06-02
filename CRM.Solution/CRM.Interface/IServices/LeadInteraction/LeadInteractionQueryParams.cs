using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.LeadInteraction
{
    public class LeadInteractionQueryParams : BaseQueryParams
    {
        public int? LeadId { get; set; }
    }
}
