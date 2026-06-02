using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.LeadSource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class LeadSourcesController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<LeadSourceReadDTO>>> GetAll([FromQuery] LeadSourceQueryParams queryParams)
        {
            var result = await _serviceManager.LeadSourceService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<LeadSourceReadDTO>> GetById(int id)
        {
            var result = await _serviceManager.LeadSourceService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<LeadSourceReadDTO>> Create([FromBody] LeadSourceCreateUpdateDTO dto)
        {
            var result = await _serviceManager.LeadSourceService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<LeadSourceReadDTO>> Update(int id, [FromBody] LeadSourceCreateUpdateDTO dto)
        {
            var result = await _serviceManager.LeadSourceService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.LeadSourceService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
