using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Dashboard
{
    public class TopAgentDTO
    {
        public string AgentName { get; set; } = default!;
        public decimal TotalSalesAmount { get; set; }
        public int CompletedDealsCount { get; set; }
    }
}
