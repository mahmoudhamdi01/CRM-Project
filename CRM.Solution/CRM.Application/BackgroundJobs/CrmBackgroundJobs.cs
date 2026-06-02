using CRM.Infrastructure.Entities.Deals;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Infrastructure.Enums;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.BackgroundJobs;
using CRM.Interface.IServices.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.BackgroundJobs
{
    public class CrmBackgroundJobs : ICrmBackgroundJobs
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<CrmBackgroundJobs> _logger;

        public CrmBackgroundJobs(
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ILogger<CrmBackgroundJobs> logger)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        // 1. القصة الأولى: الأقساط المتأخرة
        public async Task ProcessOverdueInstallmentsAsync()
        {
            _logger.LogInformation("Hangfire Job: 'ProcessOverdueInstallments' started at {Time}", DateTime.UtcNow);

            var installmentRepo = _unitOfWork.GetRepository<Installment, int>();

            // جلب الأقساط غير المدفوعة وتاريخها عدا اليوم
            var overdueInstallments = await installmentRepo.Query()
                .Include(x => x.Deal)
                .Where(x => !x.IsDeleted && x.Status == InstallmentStatus.Unpaid && x.DueDate.Date < DateTime.UtcNow.Date)
                .ToListAsync();

            if (!overdueInstallments.Any())
            {
                _logger.LogInformation("Hangfire Job: No overdue installments found today.");
                return;
            }

            foreach (var installment in overdueInstallments)
            {
                // تحديث الحالة لـ متأخر
                installment.Status = InstallmentStatus.Overdue;

                // بيزنس الإشعارات: نبعت إشعار للموظف المسؤول عن الصفقة (لو موجود) أو للأدمن
                var targetUserId = installment.Deal.AssignedUserId;
                if (!string.IsNullOrEmpty(targetUserId))
                {
                    await _notificationService.SendNotificationAsync(new NotificationCreateDTO
                    {
                        Title = "تحذير: قسط متأخر!",
                        Message = $"القسط الخاص بالصفقة رقم {installment.DealId} بمبلغ {installment.Amount} تجاوز تاريخ الاستحقاق ({installment.DueDate:yyyy-MM-dd}).",
                        Type = NotificationType.InstallmentOverdue,
                        UserId = targetUserId,
                        RelatedEntityId = installment.Id.ToString()
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Hangfire Job: Successfully updated {Count} installments to Overdue.", overdueInstallments.Count);
        }

        // 2. القصة الثانية: تذكير المتابعات اليومية
        public async Task ProcessDailyFollowUpRemindersAsync()
        {
            _logger.LogInformation("Hangfire Job: 'ProcessDailyFollowUpReminders' started at {Time}", DateTime.UtcNow);

            var interactionRepo = _unitOfWork.GetRepository<LeadInteraction, int>();

            // جلب المتابعات اللي ميعادها القادم هو النهاردة بالظبط
            var todaysInteractions = await interactionRepo.Query()
                .Include(x => x.Lead)
                .Where(x => !x.IsDeleted && x.NextFollowUpDate.HasValue && x.NextFollowUpDate.Value.Date == DateTime.UtcNow.Date)
                .ToListAsync();

            if (!todaysInteractions.Any())
            {
                _logger.LogInformation("Hangfire Job: No follow-up reminders for today.");
                return;
            }

            foreach (var interaction in todaysInteractions)
            {
                var targetUserId = interaction.Lead.AssignedUserId;

                // لو العميل مربوط بموظف مبيعات، نبعت له تذكير فوراً
                if (!string.IsNullOrEmpty(targetUserId))
                {
                    await _notificationService.SendNotificationAsync(new NotificationCreateDTO
                    {
                        Title = "تذكير بموعد متابعة عميل",
                        Message = $"لديك موعد متابعة اليوم مع العميل {interaction.Lead.FullName} بناءً على آخر تفاعل مسجل.",
                        Type = NotificationType.FollowUpReminder,
                        UserId = targetUserId,
                        RelatedEntityId = interaction.LeadId.ToString()
                    });
                }
            }

            _logger.LogInformation("Hangfire Job: Sent {Count} follow-up notifications to agents.", todaysInteractions.Count);
        }
    }
}
