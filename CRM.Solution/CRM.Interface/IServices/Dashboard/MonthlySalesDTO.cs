using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Dashboard
{
    public class MonthlySalesDTO
    {
        public string MonthName { get; set; } = default!;
        public decimal TotalSales { get; set; }
        public int DealsCount { get; set; }
    }
}
