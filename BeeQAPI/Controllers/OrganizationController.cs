using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
    //[Authorize(Roles = "Super Admin")]
    [Route("BeeQAPI")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IBAL_Organization _bal;

        public OrganizationController(IBAL_Organization bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================

        [Authorize(Policy = "VIEW_ORG")]
        [HttpPost("OrganizationList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<OrganizationModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<OrganizationModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }


        [Authorize(Policy = "VIEW_ORG")]
        [HttpPost("OrganizationById")]
        [ProducesResponseType(typeof(APIGetResponseModel<OrganizationModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<OrganizationModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }


        [Authorize(Policy = "CREATE_ORG")]
        [HttpPost("NewOrganization")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] OrganizationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }


        [Authorize(Policy = "UPDATE_ORG")]
        [HttpPost("EditOrganization")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Update([FromBody] OrganizationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);
        }


        [Authorize(Policy = "DELETE_ORG")]
        [HttpPost("OrganizationStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] OrganizationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.ChangeStatus(request.OrganizationId, roles, email, transaction: null);
        }
        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_ORG")]
        [HttpGet("OrganizationDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDropdown(email, transaction: null);
        }
    }
}