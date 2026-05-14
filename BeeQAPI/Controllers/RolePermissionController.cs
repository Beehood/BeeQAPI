using BAL.ContractIF;
using DAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly IBAL_RolePermission _bal;

        public RolePermissionController(IBAL_RolePermission bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_PERMISSION")]
        [HttpPost("RolePermissionList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<RolePermissionModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<RolePermissionModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ROLE
        // ========================
        [Authorize(Policy = "VIEW_PERMISSION")]
        [HttpPost("RolePermissionByRole")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<RolePermissionModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<RolePermissionModel>>> GetByRoleId([FromBody] long roleId)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetByRoleId(roleId, roles, email, transaction: null);
        }

        // ========================
        // CREATE (Single Assign)
        // ========================
        [Authorize(Policy = "ASSIGN_PERMISSION")]
        [HttpPost("AssignPermission")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] RolePermissionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // BULK ASSIGN (UPDATE)
        // ========================
        [Authorize(Policy = "ASSIGN_PERMISSION")]
        [HttpPost("AssignPermissionsBulk")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> BulkAssign([FromBody] RolePermissionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.BulkAssign(request, roles, email, transaction: null);
        }

        // ========================
        // DELETE
        // ========================
        [Authorize(Policy = "ASSIGN_PERMISSION")]
        [HttpPost("RemovePermission")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Delete([FromBody] RolePermissionRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Delete(request, roles, email, transaction: null);
        }
    }
}