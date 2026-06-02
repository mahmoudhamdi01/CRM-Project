using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Owner
{
    public interface IOwnerService
    {
        Task<PagedResult<OwnerReadDTO>> GetAllAsync(OwnerQueryParams queryParams);
        Task<OwnerReadDTO> GetByIdAsync(int id);
        Task<OwnerReadDTO> AddAsync(OwnerCreateUpdateDTO dto);
        Task<OwnerReadDTO> UpdateAsync(int id, OwnerCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
