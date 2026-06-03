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
    public class CounterPanelController : ControllerBase
    {
        private readonly IBAL_CounterPanel _bal;

        public CounterPanelController(
            IBAL_CounterPanel bal)
        {
            _bal = bal;
        }

        // ========================
        // DASHBOARD
        // ========================

        [Authorize(Policy = "VIEW_TOKEN")]
        [HttpPost("CounterDashboard")]
        [ProducesResponseType(typeof(APIGetResponseModel<CounterPanelDashboardModel>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<CounterPanelDashboardModel>>GetDashboard([FromBody]CounterPanelActionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDashboard(request,roles,email,transaction: null);
        }

        // ========================
        // CALL NEXT TOKEN
        // ========================

        [Authorize(Policy = "UPDATE_TOKEN")]
        [HttpPost("CounterCallNextToken")]
        [ProducesResponseType(typeof(APIGetResponseModel<CallNextTokenResponseDto>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<CallNextTokenResponseDto>>CallNextToken([FromBody]CounterPanelActionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.CallNextToken(request,roles,email,transaction: null);
        }

        // ========================
        // START SERVICE
        // ========================

        [Authorize(Policy = "UPDATE_TOKEN")]
        [HttpPost("CounterStartService")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>>StartService([FromBody]CounterPanelActionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.StartService(request,roles,email,transaction: null);
        }

        // ========================
        // COMPLETE SERVICE
        // ========================

        [Authorize(Policy = "UPDATE_TOKEN")]
        [HttpPost("CounterCompleteService")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>>CompleteService([FromBody]CounterPanelActionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.CompleteService(request,roles,email,transaction: null);
        }

        // ========================
        // SKIP TOKEN
        // ========================

        [Authorize(Policy = "UPDATE_TOKEN")]
        [HttpPost("CounterSkipToken")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>>SkipToken([FromBody]CounterPanelActionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.SkipToken(request,roles,email,transaction: null);
        }

        // ========================
        // RECALL TOKEN
        // ========================

        [Authorize(Policy = "UPDATE_TOKEN")]
        [HttpPost("CounterRecallToken")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>>RecallToken([FromBody]CounterPanelActionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.RecallToken(request,roles,email,transaction: null);
        }
    }
}