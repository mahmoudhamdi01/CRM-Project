using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.RealStateInventory;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Owner;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEntityAuditHelper _entityAuditHelper;

        public OwnerService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEntityAuditHelper entityAuditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _entityAuditHelper = entityAuditHelper;
        }

        public async Task<PagedResult<OwnerReadDTO>> GetAllAsync(OwnerQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Owner, int>();
            var query = repo.Query().AsNoTracking().Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();
                query = query.Where(x =>
                    x.FullName.Contains(search) ||
                    x.PhoneNumber.Contains(search) ||
                    (x.Email != null && x.Email.Contains(search)));
            }

            query = query.OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(queryParams, entity => _mapper.Map<OwnerReadDTO>(entity));
        }

        public async Task<OwnerReadDTO> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Owner, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Owner not found.");

            return _mapper.Map<OwnerReadDTO>(entity);
        }

        public async Task<OwnerReadDTO> AddAsync(OwnerCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Owner, int>();

            var entity = _mapper.Map<Owner>(dto);
            _entityAuditHelper.SetCreated(entity);

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<OwnerReadDTO> UpdateAsync(int id, OwnerCreateUpdateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Owner, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Owner not found.");

            _mapper.Map(dto, entity);
            _entityAuditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Owner, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                return false;

            _entityAuditHelper.SetSoftDeleted(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
