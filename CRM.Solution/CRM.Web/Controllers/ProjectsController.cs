using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class ProjectsController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<ProjectReadDTO>>> GetAll([FromQuery] ProjectQueryParams queryParams)
        {
            var result = await _serviceManager.ProjectService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<ProjectReadDTO>> GetById(int id)
        {
            var result = await _serviceManager.ProjectService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ProjectReadDTO>> Create([FromBody] ProjectCreateUpdateDTO dto)
        {
            var result = await _serviceManager.ProjectService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<ProjectReadDTO>> Update(int id, [FromBody] ProjectCreateUpdateDTO dto)
        {
            var result = await _serviceManager.ProjectService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.ProjectService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
