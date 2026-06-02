using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.RealStateInventory;
using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Property;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IEntityAuditHelper _auditHelper;

        public PropertyService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IEntityAuditHelper auditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _auditHelper = auditHelper;
        }

        public async Task<PagedResult<PropertyReadDTO>> GetAllAsync(PropertyQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<PropertyModel, int>();
            var query = repo.Query().Include(x => x.Project).Include(x => x.Owner).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
                query = query.Where(x => x.UnitCode.Contains(queryParams.Search));

            if (queryParams.ProjectId.HasValue)
                query = query.Where(x => x.ProjectId == queryParams.ProjectId.Value);

            if (queryParams.Status.HasValue)
                query = query.Where(x => x.Status == queryParams.Status.Value);

            query = query.OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(queryParams, entity =>
            {
                var dto = _mapper.Map<PropertyReadDTO>(entity);
                // جلب اسم المشروع بناءً على اللغة الحالية
                dto.ProjectName = _localizationService.GetLocalizedTitle(entity.Project);
                return dto;
            });
        }

        public async Task<PropertyReadDTO> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<PropertyModel, int>().Query()
                .Include(x => x.Project).Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Property not found.");

            var dto = _mapper.Map<PropertyReadDTO>(entity);
            dto.ProjectName = _localizationService.GetLocalizedTitle(entity.Project);
            return dto;
        }

        public async Task<PropertyReadDTO> AddAsync(PropertyCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<PropertyModel, int>();

            // التحقق من أن الكود غير مكرر في نفس المشروع
            var exists = await repo.Query().AnyAsync(x => x.ProjectId == dto.ProjectId && x.UnitCode == dto.UnitCode);
            if (exists) throw new ArgumentException("Unit code already exists in this project.");

            var entity = _mapper.Map<PropertyModel>(dto);
            _auditHelper.SetCreated(entity);

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<PropertyReadDTO> UpdateAsync(int id, PropertyCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<PropertyModel, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Property not found.");

            // [منع الحجز المزدوج - Double Booking Prevention]
            // لو بنعدل حالة الوحدة لـ (محجوزة أو مباعة) لازم نتأكد إنها ماكنتش محجوزة لحد تاني في نفس اللحظة
            if (dto.Status != PropertyStatus.Available && entity.Status != PropertyStatus.Available && entity.Status != dto.Status)
            {
                throw new InvalidOperationException($"Cannot change status to {dto.Status} because the unit is already {entity.Status}.");
            }

            _mapper.Map(dto, entity);
            _auditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();
            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<PropertyModel, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted) return false;

            if (entity.Status != PropertyStatus.Available)
                throw new InvalidOperationException("Cannot delete a reserved or sold property.");

            _auditHelper.SetSoftDeleted(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
