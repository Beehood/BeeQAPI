using BAL.ContractIF;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BeeQAPI.Controllers
{
    using BAL.ContractIF;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Net;
    using System.Security.Claims;

    namespace BeeQAPI.Controllers
    {
        [Route("permission")]
        [ApiController]
        //[Authorize]
        public class PermissionController : ControllerBase
        {
            private readonly IBAL_Permission _bal;

            public PermissionController(IBAL_Permission bal)
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
                    "PERMISSION_VIEW",
                    "PERMISSION_CREATE",
                    "PERMISSION_UPDATE",
                    "PERMISSION_STATUS"
                }
                };
            }

            // ========================
            // GET ALL
            // ========================
            //[Authorize(Policy = "PERMISSION_VIEW")]
            [HttpPost("PermissionList")]
            [ProducesResponseType(typeof(APIGetResponseModel<List<PermissionModel>>), (int)HttpStatusCode.OK)]
            public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
            {
                var user = GetUser();

                var result = await _bal.GetAll(request, user);
                return Ok(result);
            }

            // ========================
            // CREATE
            // ========================
            //[Authorize(Policy = "PERMISSION_CREATE")]
            [HttpPost("NewPermission")]
            [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
            public async Task<IActionResult> Create([FromBody] PermissionRequestDto request)
            {
                var user = GetUser();

                var result = await _bal.Create(request, user.Username, user);
                return Ok(result);
            }

            // ========================
            // UPDATE
            // ========================
            //[Authorize(Policy = "PERMISSION_UPDATE")]
            [HttpPost("EditPermission")]
            [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
            public async Task<IActionResult> Update([FromBody] PermissionRequestDto request)
            {
                var user = GetUser();

                var result = await _bal.Update(request, user.Username, user);
                return Ok(result);
            }

            // ========================
            // CHANGE STATUS
            // ========================
            //[Authorize(Policy = "PERMISSION_STATUS")]
            [HttpPost("PermissionStatus")]
            [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
            public async Task<IActionResult> ChangeStatus([FromBody] PermissionRequestDto request)
            {
                var user = GetUser();

                long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

                var result = await _bal.ChangeStatus(
                    request.PermissionId,
                    request.Status == true ? 1 : 0,
                    uid,
                    user
                );

                return Ok(result);
            }

            // ===================
            // DROPDOWN (PRODUCTION READY)
            // ===================
            [HttpGet("PermissionDropdown")]
            [Authorize(Policy = "PERMISSION_VIEW")]
            [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), 200)]
            public async Task<IActionResult> GetDropdown()
            {
                var user = HttpContext.Items["User"] as TokenUserInfo;

                var result = await _bal.GetDropdown(user);
                return Ok(result);
            }
        }
    }
}