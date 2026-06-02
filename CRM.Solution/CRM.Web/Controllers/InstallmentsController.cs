using CRM.Infrastructure.Shared.Pagination;
using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Installment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class InstallmentsController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<InstallmentReadDTO>>> GetAll([FromQuery] InstallmentQueryParams queryParams) =>
            Ok(await _serviceManager.InstallmentService.GetAllAsync(queryParams));

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<InstallmentReadDTO>> GetById(int id) =>
            Ok(await _serviceManager.InstallmentService.GetByIdAsync(id));

        [HttpPost("Create")]
        public async Task<ActionResult<InstallmentReadDTO>> Create([FromBody] InstallmentCreateUpdateDTO dto) =>
            Ok(await _serviceManager.InstallmentService.AddAsync(dto));

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<InstallmentReadDTO>> Update(int id, [FromBody] InstallmentCreateUpdateDTO dto) =>
            Ok(await _serviceManager.InstallmentService.UpdateAsync(id, dto));

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id) =>
            Ok(await _serviceManager.InstallmentService.DeleteAsync(id));
    }
}
