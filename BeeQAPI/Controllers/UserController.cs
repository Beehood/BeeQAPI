using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;

namespace BeeQAPI.Controllers
{
    [Route("user")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IBAL_User _bal;

        public UserController(IBAL_User bal)
        {
            _bal = bal;
        }

        // 🔥 Get user from middleware
        //private TokenUserInfo GetUser()
        //{
        //    return HttpContext.Items["User"] as TokenUserInfo;
        //}

        // for temporary testing without auth
        private TokenUserInfo GetUser()
        {
            return new TokenUserInfo
            {
                Username = "1",
                Permissions = new List<string>
                {
                    "USER_VIEW",
                    "USER_CREATE",
                    "USER_UPDATE",
                    "USER_STATUS"
                }
            };
        }

        // ========================
        // GET ALL
        // ========================
        //[Authorize(Policy = "USER_VIEW")]
        [HttpPost("UserList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<UserModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.GetAll(request, user);
            return Ok(result);
        }

        // ========================
        // GET BY ID
        // ========================
        //[Authorize(Policy = "USER_VIEW")]
        [HttpPost("UserById")]
        [ProducesResponseType(typeof(APIGetResponseModel<UserModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromBody] long id)
        {
            var user = GetUser();

            var result = await _bal.GetById(id, user);
            return Ok(result);
        }

        // ========================
        // CREATE
        // ========================
        //[Authorize(Policy = "USER_CREATE")]
        [HttpPost("NewUser")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] UserRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Create(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // UPDATE
        // ========================
        //[Authorize(Policy = "USER_UPDATE")]
        [HttpPost("EditUser")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UserRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Update(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        //[Authorize(Policy = "USER_STATUS")]
        [HttpPost("UserStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus([FromBody] UserRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(
                request.UserId,
                 request.Status == true ? 1 : 0,
                uid,
                user
            );

            return Ok(result);
        }

        // ===================
        // DROPDOWN (PRODUCTION READY)
        // ===================
        [HttpGet("UserDropdown")]
        [Authorize(Policy = "USER_VIEW")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), 200)]
        public async Task<IActionResult> GetDropdown()
        {
            var user = HttpContext.Items["User"] as TokenUserInfo;

            var result = await _bal.GetDropdown(user);
            return Ok(result);
        }
    }
}

