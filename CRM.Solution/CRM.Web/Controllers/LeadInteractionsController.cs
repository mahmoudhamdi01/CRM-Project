using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.LeadInteraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class LeadInteractionsController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<LeadInteractionReadDTO>>> GetAll([FromQuery] LeadInteractionQueryParams queryParams)
        {
            var result = await _serviceManager.LeadInteractionService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<LeadInteractionReadDTO>> GetById(int id)
        {
            var result = await _serviceManager.LeadInteractionService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<LeadInteractionReadDTO>> Create([FromBody] LeadInteractionCreateUpdateDTO dto)
        {
            var result = await _serviceManager.LeadInteractionService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<LeadInteractionReadDTO>> Update(int id, [FromBody] LeadInteractionCreateUpdateDTO dto)
        {
            var result = await _serviceManager.LeadInteractionService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.LeadInteractionService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
