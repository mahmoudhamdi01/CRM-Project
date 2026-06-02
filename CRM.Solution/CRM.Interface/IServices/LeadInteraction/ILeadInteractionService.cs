using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.LeadInteraction
{
    public interface ILeadInteractionService
    {
        Task<PagedResult<LeadInteractionReadDTO>> GetAllAsync(LeadInteractionQueryParams queryParams);
        Task<LeadInteractionReadDTO> GetByIdAsync(int id);
        Task<LeadInteractionReadDTO> AddAsync(LeadInteractionCreateUpdateDTO dto);
        Task<LeadInteractionReadDTO> UpdateAsync(int id, LeadInteractionCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
