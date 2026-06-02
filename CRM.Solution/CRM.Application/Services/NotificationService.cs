using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEntityAuditHelper _auditHelper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IEntityAuditHelper auditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _auditHelper = auditHelper;
        }

        public async Task<PagedResult<NotificationReadDTO>> GetMyNotificationsAsync(string userId, NotificationQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();
            var query = repo.Query().AsNoTracking().Where(x => x.UserId == userId && !x.IsDeleted);

            if (queryParams.IsRead.HasValue)
                query = query.Where(x => x.IsRead == queryParams.IsRead.Value);

            query = query.OrderByDescending(x => x.Id); // الأحدث أولاً

            return await query.ToPagedResultAsync(queryParams, entity => _mapper.Map<NotificationReadDTO>(entity));
        }

        public async Task<NotificationReadDTO> SendNotificationAsync(NotificationCreateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();
            var entity = _mapper.Map<Notification>(dto);

            _auditHelper.SetCreated(entity);
            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NotificationReadDTO>(entity);
        }

        public async Task<bool> MarkAsReadAsync(int id, string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();
            var entity = await repo.GetByIdAsync(id);

            // [Business Rule] التأكد إن الإشعار يخص الموظف الحالي ومش ممسوح
            if (entity is null || entity.IsDeleted || entity.UserId != userId)
                return false;

            entity.IsRead = true;
            entity.ReadOn = DateTime.UtcNow;
            _auditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();
            var unreadNotifications = await repo.Query()
                .Where(x => x.UserId == userId && !x.IsRead && !x.IsDeleted)
                .ToListAsync();

            if (!unreadNotifications.Any()) return false;

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadOn = DateTime.UtcNow;
                _auditHelper.SetUpdated(notification);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification, int>();
            return await repo.Query().CountAsync(x => x.UserId == userId && !x.IsRead && !x.IsDeleted);
        }
    }
}
