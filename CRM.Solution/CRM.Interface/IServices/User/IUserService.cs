using CRM.Infrastructure.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Interface.IServices.User
{
    public interface IUserService
    {
        Task<PagedResult<UserReadDTO>> GetAllAsync(UserQueryParams queryParams);
        Task<UserReadDTO> GetByIdAsync(string id);
        Task<UserReadDTO> AddAsync(UserCreateUpdateDTO dto);
        Task<UserReadDTO> UpdateAsync(string id, UserCreateUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}
