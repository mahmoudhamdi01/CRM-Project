using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Property
{
    public class PropertyCreateUpdateDTO
    {
        [Required, MaxLength(50)] 
        public string UnitCode { get; set; } = default!;
        public PropertyType Type { get; set; }
        public PropertyStatus Status { get; set; }
        public decimal Price { get; set; }
        public double Area { get; set; }
        public int ProjectId { get; set; }
        public int? OwnerId { get; set; }
    }
}
