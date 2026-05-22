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
    public class CustomerController : ControllerBase
    {
        private readonly IBAL_Customer _bal;

        public CustomerController(IBAL_Customer bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_CUSTOMER")]
        [HttpPost("CustomerList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<CustomerModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<CustomerModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================
        [Authorize(Policy = "VIEW_CUSTOMER")]
        [HttpPost("CustomerById")]
        [ProducesResponseType(typeof(APIGetResponseModel<CustomerModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<CustomerModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE
        // ========================
        [Authorize(Policy = "CREATE_CUSTOMER")]
        [HttpPost("NewCustomer")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] CustomerRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // UPDATE
        // ========================
        [Authorize(Policy = "UPDATE_CUSTOMER")]
        [HttpPost("EditCustomer")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Update([FromBody] CustomerRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);
        }

        // ========================
        // STATUS
        // ========================
        [Authorize(Policy = "DELETE_CUSTOMER")]
        [HttpPost("CustomerStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] CustomerStatusRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.ChangeStatus(request.CustomerId, roles, email, transaction: null);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_CUSTOMER")]
        [HttpGet("CustomerDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDropdown(email, transaction: null);
        }
    }
}
