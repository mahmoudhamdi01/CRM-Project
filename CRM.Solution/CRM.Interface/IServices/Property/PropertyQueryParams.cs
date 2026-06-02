using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Property
{
    public class PropertyQueryParams : BaseQueryParams
    {
        public int? ProjectId { get; set; }
        public PropertyStatus? Status { get; set; }
    }
}
