using CRM.Infrastructure.Entities.IdentityModule;
using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Entities.LeadModels
{
    public class Notification : BaseEntity<int>
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadOn { get; set; }

        // الموظف المستهدف بالإشعار
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        // حقل اختياري لربط الإشعار بكيان آخر (مثلا ID العميل أو الصفقة)
        public string? RelatedEntityId { get; set; }
    }
}
