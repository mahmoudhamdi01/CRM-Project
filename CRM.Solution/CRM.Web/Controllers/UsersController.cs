using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class UsersController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<UserReadDTO>>> GetAll([FromQuery] UserQueryParams queryParams)
        {
            var result = await _serviceManager.UserService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<UserReadDTO>> GetById(string id)
        {
            var result = await _serviceManager.UserService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserReadDTO>> Create([FromBody] UserCreateUpdateDTO dto)
        {
            var result = await _serviceManager.UserService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<UserReadDTO>> Update(string id, [FromBody] UserCreateUpdateDTO dto)
        {
            var result = await _serviceManager.UserService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var result = await _serviceManager.UserService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
