using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Deal
{
    public class DealQueryParams : BaseQueryParams
    {
        public DealStatus? Status { get; set; }
        public string? AssignedUserId { get; set; }
    }
}
