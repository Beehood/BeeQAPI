using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
    [Route("BeeQAPI")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly IBAL_Counter _bal;

        public CounterController(IBAL_Counter bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_COUNTER")]
        [HttpPost("CounterList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<CounterModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<CounterModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================
        [Authorize(Policy = "VIEW_COUNTER")]
        [HttpPost("CounterById")]
        [ProducesResponseType(typeof(APIGetResponseModel<CounterModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<CounterModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE
        // ========================
        [Authorize(Policy = "CREATE_COUNTER")]
        [HttpPost("NewCounter")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] CounterRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // UPDATE
        // ========================
        [Authorize(Policy = "UPDATE_COUNTER")]
        [HttpPost("EditCounter")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Update([FromBody] CounterRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);
        }

        // ========================
        // STATUS
        // ========================
        [Authorize(Policy = "DELETE_COUNTER")]
        [HttpPost("CounterStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] CounterStatusRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.ChangeStatus(request.CounterId, roles, email, transaction: null);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_COUNTER")]
        [HttpGet("CounterDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDropdown(email, transaction: null);
        }   
    }
}
