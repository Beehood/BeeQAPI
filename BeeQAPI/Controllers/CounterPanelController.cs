using BAL.ContractIF;
using BeeQAPI.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using MySqlX.XDevAPI.Common;
using System.Net;
using System.Security.Claims;


namespace BeeQAPI.Controllers
{
    [Route("BeeQAPI")]
    [ApiController]
    public class CounterPanelController : ControllerBase
    {
        private readonly IBAL_CounterPanel _bal;
        private readonly IHubContext<QueueHub> _hubContext;

        public CounterPanelController(
            IBAL_CounterPanel bal, IHubContext<QueueHub> hubContext)
        {
            _bal = bal;
            _hubContext = hubContext;
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
        [ProducesResponseType(typeof(APIGetResponseModel<CallNextTokenResponseDto>),
        (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<CallNextTokenResponseDto>>
        CallNextToken([FromBody] CounterPanelActionRequestDto request)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _bal.CallNextToken(request,roles,email,transaction: null);

            if (result.IsSuccess)
            {
                if (result.IsSuccess)
                {
                    await _hubContext.Clients.Group("1").SendAsync("QueueUpdated");
                }
            }

            return result;
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

            var result= await _bal.StartService(request,roles,email,transaction: null);
            if (result.IsSuccess)
            {
                if (result.IsSuccess)
                {
                    await _hubContext.Clients
                        .Group("1")
                        .SendAsync("QueueUpdated");
                }
            }

            return result;
        }

        // ========================
        // COMPLETE SERVICE
        // ========================

        [HttpPost("CounterCompleteService")]
        public async Task<APIGetResponseModel<int>> CompleteService([FromBody] CounterPanelActionRequestDto request)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _bal.CompleteService(request,roles,email,transaction: null);

            if (result.IsSuccess)
            {
                if (result.IsSuccess)
                {
                    await _hubContext.Clients
                        .Group("1")
                        .SendAsync("QueueUpdated");
                }
            }

            return result;
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

            var result = await _bal.SkipToken(request,roles,email,transaction: null);
            if (result.IsSuccess)
            {
                if (result.IsSuccess)
                {
                    await _hubContext.Clients
                        .Group("1")
                        .SendAsync("QueueUpdated");
                }
            }

            return result;
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

            var result =await _bal.RecallToken(request,roles,email,transaction: null);
          
            
                if (result.IsSuccess)
                {
                    await _hubContext.Clients
                        .Group("1")
                        .SendAsync("QueueUpdated");
                }
            

            return result;
        }
    }
}