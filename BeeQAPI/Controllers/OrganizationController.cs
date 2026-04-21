using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
    [Route("api/v1/organizations")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IBAL_Organization _bal;

        public OrganizationController(IBAL_Organization bal)
        {
            _bal = bal;
        }

        // 🔥 Get user from middleware
        private TokenUserInfo GetUser()
        {
            return HttpContext.Items["User"] as TokenUserInfo;
        }

        // ========================
        // GET ALL
        // ========================
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.GetAll(request, user);
            return Ok(result);
        }

        // ========================
        // GET BY ID
        // ========================
        [HttpPost("GetById")]
        public async Task<IActionResult> GetById([FromBody] long id)
        {
            var user = GetUser();

            var result = await _bal.GetById(id, user);
            return Ok(result);
        }

        // ========================
        // CREATE
        // ========================
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OrganizationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Create(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // UPDATE
        // ========================
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] OrganizationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Update(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] OrganizationRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(
                request.OrganizationId ?? 0,
                request.Status ?? 0,
                uid,
                user
            );

            return Ok(result);
        }
    }
}