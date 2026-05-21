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
    public class QueueController : ControllerBase
    {
        private readonly IBAL_Queue _bal;

        public QueueController(IBAL_Queue bal)
        {
            _bal = bal;
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

            return await _bal.ChangeStatus(request, roles, email, transaction: null);
        }

        // ========================
        // QUEUE DISPLAY (MONITOR)
        // ========================

        [Authorize(Policy = "VIEW_QUEUE")]
        [HttpPost("QueueDisplay")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<QueueDisplayModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<QueueDisplayModel>>> GetQueueDisplay()
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetQueueDisplay(roles, email, transaction: null);
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
