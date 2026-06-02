using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Lead;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class LeadService : ILeadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IEntityAuditHelper _auditHelper;

        public LeadService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IEntityAuditHelper auditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _auditHelper = auditHelper;
        }

        public async Task<PagedResult<LeadReadDTO>> GetAllAsync(LeadQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Lead, int>();
            var query = repo.Query()
                .Include(x => x.Source)
                .Include(x => x.AssignedUser)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
                query = query.Where(x => x.FullName.Contains(queryParams.Search) || x.PhoneNumber.Contains(queryParams.Search));

            if (queryParams.Status.HasValue)
                query = query.Where(x => x.Status == queryParams.Status.Value);

            if (!string.IsNullOrWhiteSpace(queryParams.AssignedUserId))
                query = query.Where(x => x.AssignedUserId == queryParams.AssignedUserId);

            query = query.OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(queryParams, entity =>
            {
                var dto = _mapper.Map<LeadReadDTO>(entity);
                dto.SourceName = _localizationService.GetLocalizedTitle(entity.Source);
                return dto;
            });
        }

        public async Task<LeadReadDTO> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<Lead, int>().Query()
                .Include(x => x.Source).Include(x => x.AssignedUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null || entity.IsDeleted) throw new KeyNotFoundException("Lead not found.");

            var dto = _mapper.Map<LeadReadDTO>(entity);
            dto.SourceName = _localizationService.GetLocalizedTitle(entity.Source);
            return dto;
        }

        public async Task<LeadReadDTO> AddAsync(LeadCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Lead, int>();

            // التأكد من عدم تكرار رقم التليفون (منع تكرار العملاء)
            var exists = await repo.Query().AnyAsync(x => x.PhoneNumber == dto.PhoneNumber);
            if (exists) throw new ArgumentException("A lead with this phone number already exists.");

            var entity = _mapper.Map<Lead>(dto);
            _auditHelper.SetCreated(entity);

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<LeadReadDTO> UpdateAsync(int id, LeadCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Lead, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted) throw new KeyNotFoundException("Lead not found.");

            _mapper.Map(dto, entity);
            _auditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();
            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Lead, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted) return false;

            // Business Rule: لا يمكن حذف عميل تم إتمام البيع معه
            if (entity.Status == LeadStatus.Won)
                throw new InvalidOperationException("Cannot delete a 'Won' lead.");

            _auditHelper.SetSoftDeleted(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
