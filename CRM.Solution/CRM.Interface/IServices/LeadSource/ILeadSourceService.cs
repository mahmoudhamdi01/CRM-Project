using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.LeadSource
{
    public interface ILeadSourceService
    {
        Task<PagedResult<LeadSourceReadDTO>> GetAllAsync(LeadSourceQueryParams queryParams);
        Task<LeadSourceReadDTO> GetByIdAsync(int id);
        Task<LeadSourceReadDTO> AddAsync(LeadSourceCreateUpdateDTO dto);
        Task<LeadSourceReadDTO> UpdateAsync(int id, LeadSourceCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
