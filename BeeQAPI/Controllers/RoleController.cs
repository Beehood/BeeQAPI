using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{


    [Route("BeeQAPI")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IBAL_Role _bal;

        public RoleController(IBAL_Role bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================

        [Authorize(Policy = "VIEW_ROLE")]
        [HttpPost("RoleList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<RoleModel>>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<List<RoleModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================

        [Authorize(Policy = "VIEW_ROLE")]
        [HttpPost("RoleById")]
        [ProducesResponseType(typeof(APIGetResponseModel<RoleModel>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<RoleModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE
        // ========================

        [Authorize(Policy = "CREATE_ROLE")]
        [HttpPost("NewRole")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>> Create([FromBody] RoleRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // UPDATE
        // ========================

        [Authorize(Policy = "UPDATE_ROLE")]
        [HttpPost("EditRole")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>> Update([FromBody] RoleRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);
        }

        // ========================
        // CHANGE STATUS
        // ========================

        [Authorize(Policy = "DELETE_ROLE")]
        [HttpPost("RoleStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] RoleStatusRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.ChangeStatus(request.RoleId, roles, email, transaction: null);
        }
        // ========================
        // DROPDOWN
        // ========================

        [Authorize(Policy = "VIEW_ROLE")]
        [HttpGet("RoleDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            return await _bal.GetDropdown(roles,email,transaction: null);
        }
        [Authorize(Policy = "VIEW_ROLE")]
        [HttpPost("RoleDropdownByOrganization")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> RoleDropdownByOrganization([FromBody] long organizationId)
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            return await _bal.GetDropdownByOrganization(
                organizationId,
                email,
                transaction: null);
        }
    }
}
