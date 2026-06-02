using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Dashboard
{
    public class DashboardSummaryDTO
    {
        // كروت الأرقام السريعة (KPIs)
        public decimal TotalSalesRevenue { get; set; }
        public int TotalLeadsCount { get; set; }
        public int ActivePropertiesCount { get; set; }
        public int ClosedDealsCount { get; set; }

        // القوائم التحليلية
        public List<MonthlySalesDTO> MonthlySales { get; set; } = new();
        public List<TopAgentDTO> TopAgents { get; set; } = new();
        public List<LeadStatusDistributionDTO> LeadStatuses { get; set; } = new();
    }
}
