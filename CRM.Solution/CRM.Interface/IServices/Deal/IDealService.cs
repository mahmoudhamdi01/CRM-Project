using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Deal
{
    public interface IDealService
    {
        Task<PagedResult<DealReadDTO>> GetAllAsync(DealQueryParams queryParams);
        Task<DealReadDTO> GetByIdAsync(int id);
        Task<DealReadDTO> AddAsync(DealCreateUpdateDTO dto);
        Task<DealReadDTO> UpdateAsync(int id, DealCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id); // Cancel Deal
    }
}
