using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.RealStateInventory
{
    public class Project : LocalizationEntity
    {
        public string? DeveloperName { get; set; }
        public string? Location { get; set; }

        public ICollection<PropertyModel> Properties { get; set; } = new List<PropertyModel>();
    }
}
