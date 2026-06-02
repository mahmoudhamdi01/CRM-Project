using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.RealStateInventory;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Project;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IEntityAuditHelper _entityAuditHelper;

        public ProjectService(
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

        public async Task<PagedResult<ProjectReadDTO>> GetAllAsync(ProjectQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Project, int>();
            var query = repo.Query().AsNoTracking().Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();
                query = query.Where(x =>
                    x.TitleArabic.Contains(search) ||
                    x.TitleEnglish.Contains(search) ||
                    (x.DeveloperName != null && x.DeveloperName.Contains(search)));
            }

            query = query.OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(queryParams, entity =>
            {
                var dto = _mapper.Map<ProjectReadDTO>(entity);
                dto.Title = _localizationService.GetLocalizedTitle(entity);
                dto.Description = _localizationService.GetLocalizedDescription(entity);
                return dto;
            });
        }

        public async Task<ProjectReadDTO> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Project, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Project not found.");

            var dto = _mapper.Map<ProjectReadDTO>(entity);
            dto.Title = _localizationService.GetLocalizedTitle(entity);
            dto.Description = _localizationService.GetLocalizedDescription(entity);

            return dto;
        }

        public async Task<ProjectReadDTO> AddAsync(ProjectCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Project, int>();

            var entity = _mapper.Map<Project>(dto);
            _entityAuditHelper.SetCreated(entity);

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<ProjectReadDTO> UpdateAsync(int id, ProjectCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Project, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Project not found.");

            _mapper.Map(dto, entity);
            _entityAuditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Project, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                return false;

            _entityAuditHelper.SetSoftDeleted(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
