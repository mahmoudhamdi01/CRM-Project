using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Property
{
    public class PropertyReadDTO
    {
        public int Id { get; set; }
        public string UnitCode { get; set; } = default!;
        public string TypeName { get; set; } = default!;
        public string StatusName { get; set; } = default!;
        public decimal Price { get; set; }
        public double Area { get; set; }
        public string ProjectName { get; set; } = default!;
        public string? OwnerName { get; set; }
    }
}
