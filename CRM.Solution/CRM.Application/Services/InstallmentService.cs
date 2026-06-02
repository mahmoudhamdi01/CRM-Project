using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.Deals;
using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Installment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class InstallmentService : IInstallmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEntityAuditHelper _auditHelper;

        public InstallmentService(IUnitOfWork unitOfWork, IMapper mapper, IEntityAuditHelper auditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _auditHelper = auditHelper;
        }

        public async Task<PagedResult<InstallmentReadDTO>> GetAllAsync(InstallmentQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Installment, int>();
            var query = repo.Query().AsNoTracking().Where(x => !x.IsDeleted);

            if (queryParams.DealId.HasValue)
                query = query.Where(x => x.DealId == queryParams.DealId.Value);

            if (queryParams.Status.HasValue)
                query = query.Where(x => x.Status == queryParams.Status.Value);

            query = query.OrderBy(x => x.DueDate); // ترتيب بالأقرب دفعاً

            return await query.ToPagedResultAsync(queryParams, entity => _mapper.Map<InstallmentReadDTO>(entity));
        }

        public async Task<InstallmentReadDTO> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<Installment, int>().GetByIdAsync(id);
            if (entity is null || entity.IsDeleted) throw new KeyNotFoundException("Installment not found.");
            return _mapper.Map<InstallmentReadDTO>(entity);
        }

        public async Task<InstallmentReadDTO> AddAsync(InstallmentCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Installment, int>();
            var entity = _mapper.Map<Installment>(dto);

            // لو الدفعة جاية كمدفوعة، نسجل تاريخ الدفع التلقائي
            if (entity.Status == InstallmentStatus.Paid && entity.PaidDate == null)
                entity.PaidDate = DateTime.UtcNow;

            _auditHelper.SetCreated(entity);
            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return await GetByIdAsync(entity.Id);
        }

        public async Task<InstallmentReadDTO> UpdateAsync(int id, InstallmentCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Installment, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted) throw new KeyNotFoundException("Installment not found.");

            _mapper.Map(dto, entity);

            // تأمين منطق الدفع
            if (entity.Status == InstallmentStatus.Paid && entity.PaidDate == null)
                entity.PaidDate = DateTime.UtcNow;
            else if (entity.Status != InstallmentStatus.Paid)
                entity.PaidDate = null;

            _auditHelper.SetUpdated(entity);
            await _unitOfWork.SaveChangesAsync();
            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Installment, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted) return false;

            _auditHelper.SetSoftDeleted(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
