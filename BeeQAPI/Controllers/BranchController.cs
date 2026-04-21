using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{

    [Route("api/v1/branch")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        private readonly IBAL_Branch _bal;

        public BranchController(IBAL_Branch bal)
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
        [ProducesResponseType(typeof(APIGetResponseModel<List<BranchModel>>), (int)HttpStatusCode.OK)]
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
        [ProducesResponseType(typeof(APIGetResponseModel<BranchModel>), (int)HttpStatusCode.OK)]
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
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] BranchRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Create(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // UPDATE
        // ========================
        [HttpPost("Update")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] BranchRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Update(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [HttpPost("ChangeStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus([FromBody] BranchRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(
                request.BranchId ?? 0,
                request.Status ?? 0,
                uid,
                user
            );

            return Ok(result);
        }
    }
}