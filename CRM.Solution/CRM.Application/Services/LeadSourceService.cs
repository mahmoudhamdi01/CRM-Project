using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.LeadSource;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class LeadSourceService : ILeadSourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IEntityAuditHelper _entityAuditHelper;

        public LeadSourceService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IEntityAuditHelper entityAuditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _entityAuditHelper = entityAuditHelper;
        }

        public async Task<PagedResult<LeadSourceReadDTO>> GetAllAsync(LeadSourceQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<LeadSource, int>();
            var query = repo.Query().AsNoTracking().Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();
                query = query.Where(x =>
                    x.TitleArabic.Contains(search) ||
                    x.TitleEnglish.Contains(search));
            }

            query = query.OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(queryParams, entity =>
            {
                var dto = _mapper.Map<LeadSourceReadDTO>(entity);
                dto.Title = _localizationService.GetLocalizedTitle(entity);
                dto.Description = _localizationService.GetLocalizedDescription(entity);
                return dto;
            });
        }

        public async Task<LeadSourceReadDTO> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<LeadSource, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Lead source not found.");

            var dto = _mapper.Map<LeadSourceReadDTO>(entity);
            dto.Title = _localizationService.GetLocalizedTitle(entity);
            dto.Description = _localizationService.GetLocalizedDescription(entity);

            return dto;
        }

        public async Task<LeadSourceReadDTO> AddAsync(LeadSourceCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<LeadSource, int>();

            var entity = _mapper.Map<LeadSource>(dto);
            _entityAuditHelper.SetCreated(entity);

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<LeadSourceReadDTO> UpdateAsync(int id, LeadSourceCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<LeadSource, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Lead source not found.");

            _mapper.Map(dto, entity);
            _entityAuditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<LeadSource, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                return false;

            _entityAuditHelper.SetSoftDeleted(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
