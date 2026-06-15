using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
    [Route("BeeQAPI")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IBAL_Dashboard _bal;

        public DashboardController(IBAL_Dashboard bal)
        {
            _bal = bal;
        }

        // ========================
        // DASHBOARD
        // ========================

        [Authorize(Policy = "VIEW_DASHBOARD")]
        [HttpPost("Dashboard")]
        [ProducesResponseType(typeof(APIGetResponseModel<DashboardModel>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<DashboardModel>>GetDashboard([FromBody] DashboardRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDashboard(request,roles,email,transaction: null);
        }
    }
    }