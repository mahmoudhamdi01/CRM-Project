using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Owner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class OwnersController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<OwnerReadDTO>>> GetAll([FromQuery] OwnerQueryParams queryParams)
        {
            var result = await _serviceManager.OwnerService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<OwnerReadDTO>> GetById(int id)
        {
            var result = await _serviceManager.OwnerService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<OwnerReadDTO>> Create([FromBody] OwnerCreateUpdateDTO dto)
        {
            var result = await _serviceManager.OwnerService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<OwnerReadDTO>> Update(int id, [FromBody] OwnerCreateUpdateDTO dto)
        {
            var result = await _serviceManager.OwnerService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.OwnerService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
