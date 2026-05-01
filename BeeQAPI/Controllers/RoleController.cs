using BAL.ContractIF;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BeeQAPI.Controllers
{
    [Route("role")]
    [ApiController]
    //[Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IBAL_Role _bal;

        public RoleController(IBAL_Role bal)
        {
            _bal = bal;
        }

        // for temporary testing without auth
        private TokenUserInfo GetUser()
        {
            return new TokenUserInfo
            {
                Username = "1",
                Permissions = new List<string>
                {
                    "ROLE_VIEW",
                    "ROLE_CREATE",
                    "ROLE_UPDATE",
                    "ROLE_STATUS"
                }
            };
        }

        // ========================
        // GET ALL
        // ========================
        [HttpPost("RoleList")]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.GetAll(request, user);
            return Ok(result);
        }

        // ========================
        // GET BY ID
        // ========================
        [HttpPost("RoleById")]
        public async Task<IActionResult> GetById([FromBody] long id)
        {
            var user = GetUser();

            var result = await _bal.GetById(id, user);
            return Ok(result);
        }

        // ========================
        // CREATE
        // ========================
        [HttpPost("NewRole")]
        public async Task<IActionResult> Create([FromBody] RoleRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Create(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // UPDATE
        // ========================
        [HttpPost("EditRole")]
        public async Task<IActionResult> Update([FromBody] RoleRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Update(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [HttpPost("RoleStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] RoleRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(
                request.RoleId,
                request.Status ?? 0,
                uid,
                user
            );

            return Ok(result);
        }

        // ========================
        // DROPDOWN
        // ========================
        [HttpPost("RoleDropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            var user = GetUser();

            var result = await _bal.GetDropdown(user);
            return Ok(result);
        }
    }
}

