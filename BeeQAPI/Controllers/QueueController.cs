using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BeeQAPI.Realtime;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
    [Route("BeeQAPI")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IBAL_Queue _bal;
        private readonly IMonitorService _monitorService;
        private readonly IHubContext<QueueHub> _hub;

        public QueueController(IBAL_Queue bal, IMonitorService monitorService, IHubContext<QueueHub> hub)
        {
            _bal = bal;
            _monitorService = monitorService;
            _hub = hub;
        }
        // ========================
        // GET ALL
        // ========================

        [Authorize(Policy = "VIEW_QUEUE")]
        [HttpPost("QueueList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<QueueModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<QueueModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================

        [Authorize(Policy = "VIEW_QUEUE")]
        [HttpPost("QueueById")]
        [ProducesResponseType(typeof(APIGetResponseModel<QueueModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<QueueModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE TOKEN
        // ========================

        [Authorize(Policy = "CREATE_QUEUE")]
        [HttpPost("NewToken")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] QueueRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // UPDATE
        // ========================

        [Authorize(Policy = "UPDATE_QUEUE")]
        [HttpPost("EditToken")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Update([FromBody] QueueRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);
        }

        // ========================
        // CHANGE STATUS (CALL / COMPLETE / TRANSFER)
        // ========================

        [Authorize(Policy = "UPDATE_QUEUE")]
        [HttpPost("QueueStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] QueueRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //  Update DB
            var response = await _bal.ChangeStatus(request, roles, email);

            //  SEND REALTIME UPDATE
            if (response.IsSuccess)
            {
                var displayData = await _bal.GetQueueDisplay(request.BranchId.ToString());

                await _hub.Clients.Group(request.BranchId.ToString()).SendAsync("QueueUpdated", displayData.Result);
            }

            return response;
        }



        // ========================
        // QUEUE DISPLAY (MONITOR)
        // ========================
        [AllowAnonymous]
        [HttpGet("Monitor/{monitorKey}")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<QueueDisplayModel>>), 200)]
        public async Task<APIGetResponseModel<List<QueueDisplayModel>>> GetQueueDisplay(string monitorKey)
        {
            var branchId = await _monitorService.GetBranchByKey(monitorKey);
            return await _bal.GetQueueDisplay(branchId);
        }
        // ========================
        // DROPDOWN
        // ========================

        [Authorize(Policy = "VIEW_QUEUE")]
        [HttpGet("QueueDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDropdown(email, transaction: null);
        }
    }

}
