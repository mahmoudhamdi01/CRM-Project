using CRM.Infrastructure.Entities;
using CRM.Interface.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Repositories
{
    public class EntityAuditHelper : IEntityAuditHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EntityAuditHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                   ?? "System";
        }

        public void SetCreated<TKey>(BaseEntity<TKey> entity)
        {
            entity.CreatedBy = GetCurrentUserId();
            entity.CreatedOn = DateTime.UtcNow;
            entity.LastModifiedBy = null;
            entity.LastModifiedOn = null;
            entity.IsDeleted = false;
        }

        public void SetUpdated<TKey>(BaseEntity<TKey> entity)
        {
            entity.LastModifiedBy = GetCurrentUserId();
            entity.LastModifiedOn = DateTime.UtcNow;
        }

        public void SetSoftDeleted<TKey>(BaseEntity<TKey> entity)
        {
            entity.IsDeleted = true;
            entity.LastModifiedBy = GetCurrentUserId();
            entity.LastModifiedOn = DateTime.UtcNow;
        }
    }
}
