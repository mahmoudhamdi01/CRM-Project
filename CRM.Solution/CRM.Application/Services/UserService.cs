using AutoMapper;
using CRM.Application.Helpers;
using CRM.Infrastructure.Entities.IdentityModule;
using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEntityAuditHelper _auditHelper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IEntityAuditHelper auditHelper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _auditHelper = auditHelper;
        }

        public async Task<PagedResult<UserReadDTO>> GetAllAsync(UserQueryParams queryParams)
        {
            var query = _userManager.Users
                .Include(x => x.Manager)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();
                query = query.Where(x =>
                    x.DisplayName.Contains(search) ||
                    x.Email.Contains(search));
            }

            query = query.OrderByDescending(x => x.CreatedOn);

            return await query.ToPagedResultAsync(queryParams, entity => _mapper.Map<UserReadDTO>(entity));
        }

        public async Task<UserReadDTO> GetByIdAsync(string id)
        {
            var entity = await _userManager.Users
                .Include(x => x.Manager)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("User not found.");

            return _mapper.Map<UserReadDTO>(entity);
        }

        public async Task<UserReadDTO> AddAsync(UserCreateUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Password is required for creation.");

            var entity = _mapper.Map<ApplicationUser>(dto);

            // Audit Setup (Since we are not using BaseEntity directly here)
            entity.CreatedBy = _auditHelper.GetCurrentUserId();
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsDeleted = false;

            var result = await _userManager.CreateAsync(entity, dto.Password);

            if (!result.Succeeded)
                throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));

            return await GetByIdAsync(entity.Id);
        }

        public async Task<UserReadDTO> UpdateAsync(string id, UserCreateUpdateDTO dto)
        {
            var entity = await _userManager.FindByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("User not found.");

            if (dto.ManagerId == id)
                throw new ArgumentException("User cannot be their own manager.");

            _mapper.Map(dto, entity);

            entity.LastModifiedBy = _auditHelper.GetCurrentUserId();
            entity.LastModifiedOn = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
                throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Update Password if provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(entity);
                await _userManager.ResetPasswordAsync(entity, token, dto.Password);
            }

            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _userManager.FindByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                return false;

            // Soft Delete
            entity.IsDeleted = true;
            entity.LastModifiedBy = _auditHelper.GetCurrentUserId();
            entity.LastModifiedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(entity);

            return true;
        }
    }
}
