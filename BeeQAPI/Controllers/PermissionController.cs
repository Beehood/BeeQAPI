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
    public class PermissionController : ControllerBase
    {
        private readonly IBAL_Permission _bal;

        public PermissionController(IBAL_Permission bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_PERMISSION")]
        [HttpPost("PermissionList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<PermissionModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<PermissionModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================
        [Authorize(Policy = "VIEW_PERMISSION")]
        [HttpPost("PermissionById")]
        [ProducesResponseType(typeof(APIGetResponseModel<PermissionModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<PermissionModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE
        // ========================
        [Authorize(Policy = "CREATE_PERMISSION")]
        [HttpPost("NewPermission")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] PermissionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // UPDATE
        // ========================
        [Authorize(Policy = "UPDATE_PERMISSION")]
        [HttpPost("EditPermission")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Update([FromBody] PermissionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.Update(request, roles, email, transaction: null);
        }

        // ========================
        // STATUS
        // ========================
        [Authorize(Policy = "DELETE_PERMISSION")]
        [HttpPost("PermissionStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] PermissionStatusRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.ChangeStatus(request.PermissionId, roles, email, transaction: null);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_PERMISSION")]
        [HttpGet("PermissionDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.GetDropdown(email, transaction: null);
        }
    }
}