using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.RealStateInventory
{
    public class Owner : BaseEntity<int>
    {
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string? Email { get; set; }
        public string? NationalId { get; set; }

        public ICollection<PropertyModel> Properties { get; set; } = new List<PropertyModel>();
    }
}
