using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class DashboardController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetSummary")]
        public async Task<ActionResult<DashboardSummaryDTO>> GetSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var result = await _serviceManager.DashboardService.GetDashboardSummaryAsync(startDate, endDate);
            return Ok(result);
        }
    }
}
