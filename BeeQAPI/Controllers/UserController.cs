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
    public class UserController : ControllerBase
    {
        private readonly IBAL_User _bal;

        public UserController(IBAL_User bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================

        [Authorize(Policy = "VIEW_USER")]
        [HttpPost("UserList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<UserModel>>),(int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<List<UserModel>>>GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request,roles,email,transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================

        [Authorize(Policy = "VIEW_USER")]
        [HttpPost("UserById")]
        [ProducesResponseType(typeof(APIGetResponseModel<UserModel>),(int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<UserModel>>GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id,roles,email,transaction: null);
        }

        // ========================
        // CREATE
        // ========================

        [Authorize(Policy = "CREATE_USER")]
        [HttpPost("NewUser")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>),(int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>>Create([FromBody] UserRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request,roles,email,transaction: null);
        }

        // ========================
        // UPDATE
        // ========================

        [Authorize(Policy = "UPDATE_USER")]
        [HttpPost("EditUser")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>),(int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>>Update([FromBody] UserRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;return await _bal.Update(request,roles,email,transaction: null);
        }

        // ========================
        // CHANGE STATUS
        // ========================

        [Authorize(Policy = "DELETE_USER")]
        [HttpPost("UserStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>),(int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>>ChangeStatus([FromBody] UserRequestDto request
        )
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.ChangeStatus(request.UserId,roles,email,transaction: null);
        }

        // ========================
        // DROPDOWN
        // ========================

        [Authorize(Policy = "VIEW_USER")]
        [HttpGet("UserDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>),(int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<List<DropdownModel>>>GetDropdown()
        {
            var email =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.GetDropdown(email,transaction: null);
        }
    }
}