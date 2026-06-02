using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Notification
{
    public class NotificationReadDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string TypeName { get; set; } = default!;
        public bool IsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? RelatedEntityId { get; set; }
    }
}
