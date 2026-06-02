using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Lead
{
    public interface ILeadService
    {
        Task<PagedResult<LeadReadDTO>> GetAllAsync(LeadQueryParams queryParams);
        Task<LeadReadDTO> GetByIdAsync(int id);
        Task<LeadReadDTO> AddAsync(LeadCreateUpdateDTO dto);
        Task<LeadReadDTO> UpdateAsync(int id, LeadCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
