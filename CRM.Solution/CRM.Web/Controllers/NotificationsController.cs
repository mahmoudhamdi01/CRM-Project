using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class NotificationsController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetMyNotifications")]
        public async Task<ActionResult<PagedResult<NotificationReadDTO>>> GetMyNotifications([FromQuery] NotificationQueryParams queryParams)
        {
            var result = await _serviceManager.NotificationService.GetMyNotificationsAsync(GetName(), queryParams);
            return Ok(result);
        }

        [HttpGet("GetUnreadCount")]
        public async Task<ActionResult<int>> GetUnreadCount()
        {
            var count = await _serviceManager.NotificationService.GetUnreadCountAsync(GetName());
            return Ok(count);
        }

        [HttpPut("MarkAsRead/{id:int}")]
        public async Task<ActionResult<bool>> MarkAsRead(int id)
        {
            var result = await _serviceManager.NotificationService.MarkAsReadAsync(id, GetName());
            return Ok(result);
        }

        [HttpPut("MarkAllAsRead")]
        public async Task<ActionResult<bool>> MarkAllAsRead()
        {
            var result = await _serviceManager.NotificationService.MarkAllAsReadAsync(GetName());
            return Ok(result);
        }

        // إندبوينت تست للأدمن عشان يبعت إشعار يدوي لموظف
        [HttpPost("SendManual")]
        public async Task<ActionResult<NotificationReadDTO>> SendManual([FromBody] NotificationCreateDTO dto) =>
            Ok(await _serviceManager.NotificationService.SendNotificationAsync(dto));
    }
}
