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
            [HttpPost("PermissionList")]
            public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
            {
                var user = GetUser();

                var result = await _bal.GetAll(request, user);
                return Ok(result);
            }

            // ========================
            // CREATE
            // ========================
            [HttpPost("NewPermission")]
            public async Task<IActionResult> Create([FromBody] PermissionRequestDto request)
            {
                var user = GetUser();

                var result = await _bal.Create(request, user.Username, user);
                return Ok(result);
            }

            // ========================
            // UPDATE
            // ========================
            [HttpPost("EditPermission")]
            public async Task<IActionResult> Update([FromBody] PermissionRequestDto request)
            {
                var user = GetUser();

                var result = await _bal.Update(request, user.Username, user);
                return Ok(result);
            }

            // ========================
            // CHANGE STATUS
            // ========================
            [HttpPost("PermissionStatus")]
            public async Task<IActionResult> ChangeStatus([FromBody] PermissionRequestDto request)
            {
                var user = GetUser();

                long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

                var result = await _bal.ChangeStatus(
                    request.PermissionId,
                    request.Status ?? 0,
                    uid,
                    user
                );

                return Ok(result);
            }

            // ========================
            // DROPDOWN
            // ========================
            [HttpPost("PermissionDropdown")]
            public async Task<IActionResult> GetDropdown()
            {
                var user = GetUser();

                var result = await _bal.GetDropdown(user);
                return Ok(result);
            }
        }
    }
}