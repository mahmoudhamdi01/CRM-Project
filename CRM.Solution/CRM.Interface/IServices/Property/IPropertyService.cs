using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Property
{
    public interface IPropertyService
    {
        Task<PagedResult<PropertyReadDTO>> GetAllAsync(PropertyQueryParams queryParams);
        Task<PropertyReadDTO> GetByIdAsync(int id);
        Task<PropertyReadDTO> AddAsync(PropertyCreateUpdateDTO dto);
        Task<PropertyReadDTO> UpdateAsync(int id, PropertyCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
