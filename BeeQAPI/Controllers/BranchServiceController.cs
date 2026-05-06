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
    public class BranchServiceController : ControllerBase
    {
        private readonly IBAL_BranchService _bal;

        public BranchServiceController(IBAL_BranchService bal)
        {
            _bal = bal;
        }
        //=================

        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_BRANCH_SERVICE")]
        [HttpPost("BranchServiceList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<BranchServiceModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================
        [Authorize(Policy = "VIEW_BRANCH_SERVICE")]
        [HttpPost("BranchServiceById")]
        [ProducesResponseType(typeof(APIGetResponseModel<BranchServiceModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<BranchServiceModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE
        // ========================
        [Authorize(Policy = "CREATE_BRANCH_SERVICE")]
        [HttpPost("NewBranchService")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] BranchServiceRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // UPDATE
        // ========================
        [Authorize(Policy = "UPDATE_BRANCH_SERVICE")]
        [HttpPost("EditBranchService")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Update([FromBody] BranchServiceRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);
        }

        // ========================
        // CHANGE STATUS (DELETE)
        // ========================
        [Authorize(Policy = "DELETE_BRANCH_SERVICE")]
        [HttpPost("BranchServiceStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.ChangeStatus(id, roles, email, transaction: null);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_BRANCH_SERVICE")]
        [HttpGet("BranchServiceDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDropdown(email, transaction: null);
        }
    }
}