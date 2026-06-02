using CRM.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Notification
{
    public class NotificationCreateDTO
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;
        [Required, MaxLength(1000)] 
        public string Message { get; set; } = default!;
        public NotificationType Type { get; set; }
        public string UserId { get; set; } = default!;
        public string? RelatedEntityId { get; set; }
    }
}
