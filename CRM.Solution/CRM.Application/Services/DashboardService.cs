using CRM.Infrastructure.Entities.Deals;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Infrastructure.Entities.RealStateInventory;
using CRM.Infrastructure.Enums;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? DateTime.UtcNow.AddMonths(-6); // الافتراضي آخر 6 شهور
            var end = endDate ?? DateTime.UtcNow;

            var dealRepo = _unitOfWork.GetRepository<Deal, int>();
            var leadRepo = _unitOfWork.GetRepository<Lead, int>();
            var propertyRepo = _unitOfWork.GetRepository<PropertyModel, int>();

            // 1. حساب الـ KPIs الأساسية
            var totalSalesRevenue = await dealRepo.Query()
                .Where(x => !x.IsDeleted && x.Status != DealStatus.Cancelled && x.DealDate >= start && x.DealDate <= end)
                .SumAsync(x => x.TotalAmount);

            var closedDealsCount = await dealRepo.Query()
                .Where(x => !x.IsDeleted && x.Status == DealStatus.Approved && x.DealDate >= start && x.DealDate <= end)
                .CountAsync();

            var totalLeadsCount = await leadRepo.Query()
                .Where(x => !x.IsDeleted && x.CreatedOn >= start && x.CreatedOn <= end)
                .CountAsync();

            var activePropertiesCount = await propertyRepo.Query()
                .Where(x => !x.IsDeleted && x.Status == PropertyStatus.Available)
                .CountAsync();

            var monthlySalesRaw = await dealRepo.Query()
                .Where(x => !x.IsDeleted && x.Status != DealStatus.Cancelled && x.DealDate >= start && x.DealDate <= end)
                .GroupBy(x => new { x.DealDate.Year, x.DealDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSales = g.Sum(x => x.TotalAmount),
                    DealsCount = g.Count()
                })
                .ToListAsync(); // هنا البيانات رجعت للذاكرة

            // ثانياً: نعمل التنسيق اللي إحنا عايزينه (Client-side)
            var monthlySalesData = monthlySalesRaw
                .Select(x => new MonthlySalesDTO
                {
                    MonthName = $"{x.Year}-{x.Month:D2}",
                    TotalSales = x.TotalSales,
                    DealsCount = x.DealsCount
                })
                .OrderBy(x => x.MonthName)
                .ToList();


            // 3. أعلى موظفي مبيعات تحقيقاً للأرباح (Top Performing Agents)
            var topAgentsData = await dealRepo.Query()
                .Where(x => !x.IsDeleted && x.Status == DealStatus.Approved && x.AssignedUserId != null && x.DealDate >= start && x.DealDate <= end)
                .GroupBy(x => x.AssignedUser!.DisplayName)
                .Select(g => new TopAgentDTO
                {
                    AgentName = g.Key,
                    TotalSalesAmount = g.Sum(x => x.TotalAmount),
                    CompletedDealsCount = g.Count()
                })
                .OrderByDescending(x => x.TotalSalesAmount)
                .Take(5) // أعلى 5 موظفين
                .ToListAsync();

            // 4. توزيع حالات العملاء (Lead Status Distribution)
            var leadStatusesData = await leadRepo.Query()
                .Where(x => !x.IsDeleted && x.CreatedOn >= start && x.CreatedOn <= end)
                .GroupBy(x => x.Status)
                .Select(g => new LeadStatusDistributionDTO
                {
                    StatusName = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync();

            // تجميع كل البيانات في الـ Dashboard DTO
            return new DashboardSummaryDTO
            {
                TotalSalesRevenue = totalSalesRevenue,
                TotalLeadsCount = totalLeadsCount,
                ActivePropertiesCount = activePropertiesCount,
                ClosedDealsCount = closedDealsCount,
                MonthlySales = monthlySalesData,
                TopAgents = topAgentsData,
                LeadStatuses = leadStatusesData
            };
        }
    }
}
