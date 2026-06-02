using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.LeadModels
{
    public class LeadSource : LocalizationEntity
    {
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
