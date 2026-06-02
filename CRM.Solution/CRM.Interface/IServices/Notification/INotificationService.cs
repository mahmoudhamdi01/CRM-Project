using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Notification
{
    public interface INotificationService
    {
        Task<PagedResult<NotificationReadDTO>> GetMyNotificationsAsync(string userId, NotificationQueryParams queryParams);
        Task<NotificationReadDTO> SendNotificationAsync(NotificationCreateDTO dto);
        Task<bool> MarkAsReadAsync(int id, string userId);
        Task<bool> MarkAllAsReadAsync(string userId);
        Task<int> GetUnreadCountAsync(string userId);
    }
}
