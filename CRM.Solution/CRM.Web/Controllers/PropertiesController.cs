using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Property;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class PropertiesController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<PropertyReadDTO>>> GetAll([FromQuery] PropertyQueryParams queryParams)
        {
            var result = await _serviceManager.PropertyService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<PropertyReadDTO>> GetById(int id)
        {
            var result = await _serviceManager.PropertyService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<PropertyReadDTO>> Create([FromBody] PropertyCreateUpdateDTO dto)
        {
            var result = await _serviceManager.PropertyService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<PropertyReadDTO>> Update(int id, [FromBody] PropertyCreateUpdateDTO dto)
        {
            var result = await _serviceManager.PropertyService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.PropertyService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
