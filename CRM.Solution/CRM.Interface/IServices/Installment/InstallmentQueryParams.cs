using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Installment
{
    public class InstallmentQueryParams : BaseQueryParams
    {
        public int? DealId { get; set; }
        public InstallmentStatus? Status { get; set; }
    }
}
