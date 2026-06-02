using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.RealStateInventory
{
    public class PropertyModel : BaseEntity<int>
    {
        public string UnitCode { get; set; } = default!;
        public PropertyType Type { get; set; }
        public PropertyStatus Status { get; set; }
        public decimal Price { get; set; }
        public double Area { get; set; }

        // العلاقات
        public int ProjectId { get; set; }
        public Project Project { get; set; } = default!;

        public int? OwnerId { get; set; } // Nullable في حالة البيع الأولي (Primary)
        public Owner? Owner { get; set; }
    }
}
