using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.Deals;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Infrastructure.Entities.RealStateInventory;
using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Deal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class DealService : IDealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEntityAuditHelper _auditHelper;

        public DealService(IUnitOfWork unitOfWork, IMapper mapper, IEntityAuditHelper auditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _auditHelper = auditHelper;
        }

        public async Task<PagedResult<DealReadDTO>> GetAllAsync(DealQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Deal, int>();
            var query = repo.Query().Include(x => x.Lead).Include(x => x.Property).Include(x => x.AssignedUser).AsNoTracking().Where(x => !x.IsDeleted);

            if (queryParams.Status.HasValue)
                query = query.Where(x => x.Status == queryParams.Status.Value);

            if (!string.IsNullOrWhiteSpace(queryParams.AssignedUserId))
                query = query.Where(x => x.AssignedUserId == queryParams.AssignedUserId);

            query = query.OrderByDescending(x => x.DealDate);

            return await query.ToPagedResultAsync(queryParams, entity => _mapper.Map<DealReadDTO>(entity));
        }

        public async Task<DealReadDTO> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<Deal, int>().Query()
                .Include(x => x.Lead).Include(x => x.Property).Include(x => x.AssignedUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null || entity.IsDeleted) throw new KeyNotFoundException("Deal not found.");
            return _mapper.Map<DealReadDTO>(entity);
        }

        public async Task<DealReadDTO> AddAsync(DealCreateUpdateDTO dto)
        {
            var propertyRepo = _unitOfWork.GetRepository<PropertyModel, int>();
            var leadRepo = _unitOfWork.GetRepository<Lead, int>();
            var dealRepo = _unitOfWork.GetRepository<Deal, int>();

            var property = await propertyRepo.GetByIdAsync(dto.PropertyId);
            if (property == null || property.IsDeleted) throw new KeyNotFoundException("Property not found.");

            // [Business Rule] التأكد إن الوحدة مش متباعة لعميل تاني
            if (property.Status == PropertyStatus.Sold)
                throw new InvalidOperationException("This property is already sold.");

            var deal = _mapper.Map<Deal>(dto);
            _auditHelper.SetCreated(deal);
            await dealRepo.AddAsync(deal);

            // [Business Rule] تحويل حالة الوحدة لـ مباعة
            property.Status = PropertyStatus.Sold;
            _auditHelper.SetUpdated(property);

            // [Business Rule] تحويل حالة العميل لـ Won
            var lead = await leadRepo.GetByIdAsync(dto.LeadId);
            if (lead != null && lead.Status != LeadStatus.Won)
            {
                lead.Status = LeadStatus.Won;
                _auditHelper.SetUpdated(lead);
            }

            await _unitOfWork.SaveChangesAsync();
            return await GetByIdAsync(deal.Id);
        }

        public async Task<DealReadDTO> UpdateAsync(int id, DealCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Deal, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted) throw new KeyNotFoundException("Deal not found.");

            // [Business Rule] لو الديل اتلغى، نرجع الوحدة متاحة
            if (dto.Status == DealStatus.Cancelled && entity.Status != DealStatus.Cancelled)
            {
                var propertyRepo = _unitOfWork.GetRepository<PropertyModel, int>();
                var property = await propertyRepo.GetByIdAsync(entity.PropertyId);
                if (property != null)
                {
                    property.Status = PropertyStatus.Available;
                    _auditHelper.SetUpdated(property);
                }
            }

            _mapper.Map(dto, entity);
            _auditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();
            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Deal, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted) return false;

            _auditHelper.SetSoftDeleted(entity);

            // نرجع الوحدة لـ متاحة لو مسحنا الديل بالغلط أو اتلغى
            var propertyRepo = _unitOfWork.GetRepository<PropertyModel, int>();
            var property = await propertyRepo.GetByIdAsync(entity.PropertyId);
            if (property != null)
            {
                property.Status = PropertyStatus.Available;
                _auditHelper.SetUpdated(property);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
