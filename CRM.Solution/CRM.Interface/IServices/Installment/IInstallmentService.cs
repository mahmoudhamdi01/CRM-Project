using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Installment
{
    public interface IInstallmentService
    {
        Task<PagedResult<InstallmentReadDTO>> GetAllAsync(InstallmentQueryParams queryParams);
        Task<InstallmentReadDTO> GetByIdAsync(int id);
        Task<InstallmentReadDTO> AddAsync(InstallmentCreateUpdateDTO dto);
        Task<InstallmentReadDTO> UpdateAsync(int id, InstallmentCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
