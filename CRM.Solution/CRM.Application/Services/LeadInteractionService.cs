using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.LeadModels;
using CRM.Infrastructure.Enums;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.LeadInteraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class LeadInteractionService : ILeadInteractionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEntityAuditHelper _entityAuditHelper;

        public LeadInteractionService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEntityAuditHelper entityAuditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _entityAuditHelper = entityAuditHelper;
        }

        public async Task<PagedResult<LeadInteractionReadDTO>> GetAllAsync(LeadInteractionQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<LeadInteraction, int>();
            var query = repo.Query().Include(x => x.Lead).AsNoTracking().Where(x => !x.IsDeleted);

            // فلترة بالملاحظات (Search)
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();
                query = query.Where(x => x.Notes != null && x.Notes.Contains(search));
            }

            // فلترة بالعميل نفسه
            if (queryParams.LeadId.HasValue)
            {
                query = query.Where(x => x.LeadId == queryParams.LeadId.Value);
            }

            query = query.OrderByDescending(x => x.InteractionDate);

            return await query.ToPagedResultAsync(queryParams, entity => _mapper.Map<LeadInteractionReadDTO>(entity));
        }

        public async Task<LeadInteractionReadDTO> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<LeadInteraction, int>();
            var entity = await repo.Query().Include(x => x.Lead).FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Lead interaction not found.");

            return _mapper.Map<LeadInteractionReadDTO>(entity);
        }

        public async Task<LeadInteractionReadDTO> AddAsync(LeadInteractionCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<LeadInteraction, int>();

            var entity = _mapper.Map<LeadInteraction>(dto);
            _entityAuditHelper.SetCreated(entity);

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            // تحديث حالة العميل التلقائي (اختياري كـ Business Logic)
            // لو دي أول مكالمة معاه، ممكن نغير حالة العميل من New لـ Contacted
            var leadRepo = _unitOfWork.GetRepository<Lead, int>();
            var lead = await leadRepo.GetByIdAsync(dto.LeadId);
            if (lead != null && lead.Status == LeadStatus.New)
            {
                lead.Status = LeadStatus.Contacted;
                _entityAuditHelper.SetUpdated(lead);
                await _unitOfWork.SaveChangesAsync();
            }

            return await GetByIdAsync(entity.Id);
        }

        public async Task<LeadInteractionReadDTO> UpdateAsync(int id, LeadInteractionCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<LeadInteraction, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Lead interaction not found.");

            _mapper.Map(dto, entity);
            _entityAuditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<LeadInteraction, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                return false;

            _entityAuditHelper.SetSoftDeleted(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
