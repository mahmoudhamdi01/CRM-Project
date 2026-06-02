using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Dashboard
{
    public class LeadStatusDistributionDTO
    {
        public string StatusName { get; set; } = default!;
        public int Count { get; set; }
    }
}
