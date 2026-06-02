using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.Project
{
    public interface IProjectService
    {
        Task<PagedResult<ProjectReadDTO>> GetAllAsync(ProjectQueryParams queryParams);
        Task<ProjectReadDTO> GetByIdAsync(int id);
        Task<ProjectReadDTO> AddAsync(ProjectCreateUpdateDTO dto);
        Task<ProjectReadDTO> UpdateAsync(int id, ProjectCreateUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
