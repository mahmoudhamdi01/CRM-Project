using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Lead;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class LeadsController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<LeadReadDTO>>> GetAll([FromQuery] LeadQueryParams queryParams) =>
            Ok(await _serviceManager.LeadService.GetAllAsync(queryParams));

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<LeadReadDTO>> GetById(int id) =>
            Ok(await _serviceManager.LeadService.GetByIdAsync(id));

        [HttpPost("Create")]
        public async Task<ActionResult<LeadReadDTO>> Create([FromBody] LeadCreateUpdateDTO dto) =>
            Ok(await _serviceManager.LeadService.AddAsync(dto));

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<LeadReadDTO>> Update(int id, [FromBody] LeadCreateUpdateDTO dto) =>
            Ok(await _serviceManager.LeadService.UpdateAsync(id, dto));

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id) =>
            Ok(await _serviceManager.LeadService.DeleteAsync(id));
    }
}
