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
    public class BranchDeviceController : ControllerBase
    {
        private readonly IBAL_BranchDevice _bal;

        public BranchDeviceController(IBAL_BranchDevice bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_DEVICE")]
        [HttpPost("BranchDeviceList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DeviceModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DeviceModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================
        [Authorize(Policy = "VIEW_DEVICE")]
        [HttpPost("BranchDeviceById")]
        [ProducesResponseType(typeof(APIGetResponseModel<DeviceModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<DeviceModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE
        // ========================
        [Authorize(Policy = "CREATE_DEVICE")]
        [HttpPost("NewBranchDevice")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] DeviceRequestDto request)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // UPDATE
        // ========================
        [Authorize(Policy = "UPDATE_DEVICE")]
        [HttpPost("EditBranchDevice")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Update([FromBody] DeviceRequestDto request)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);
        }

        // ========================
        // STATUS
        // ========================
        [Authorize(Policy = "DELETE_DEVICE")]
        [HttpPost("BranchDeviceStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] DeviceStatusRequestDto request)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.ChangeStatus(request.DeviceId, roles, email, transaction: null);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_DEVICE")]
        [HttpGet("BranchDeviceDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDropdown(email, transaction: null);
        }
    }
}
