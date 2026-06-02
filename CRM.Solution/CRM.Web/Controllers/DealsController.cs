using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Deal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class DealsController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<DealReadDTO>>> GetAll([FromQuery] DealQueryParams queryParams) =>
            Ok(await _serviceManager.DealService.GetAllAsync(queryParams));

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<DealReadDTO>> GetById(int id) =>
            Ok(await _serviceManager.DealService.GetByIdAsync(id));

        [HttpPost("Create")]
        public async Task<ActionResult<DealReadDTO>> Create([FromBody] DealCreateUpdateDTO dto) =>
            Ok(await _serviceManager.DealService.AddAsync(dto));

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<DealReadDTO>> Update(int id, [FromBody] DealCreateUpdateDTO dto) =>
            Ok(await _serviceManager.DealService.UpdateAsync(id, dto));

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id) =>
            Ok(await _serviceManager.DealService.DeleteAsync(id));
    }
}
