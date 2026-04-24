using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{

    [Route("Branch")]
    [ApiController]
    //[Authorize]
    public class BranchController : ControllerBase
    {
        private readonly IBAL_Branch _bal;

        public BranchController(IBAL_Branch bal)
        {
            _bal = bal;
        }

        // 🔥 Get user from middleware
        //private TokenUserInfo GetUser()
        //{
        //    return HttpContext.Items["User"] as TokenUserInfo;
        //}
        //for temporaly testing without auth//
        private TokenUserInfo GetUser()
        {
            return new TokenUserInfo
            {
                Username = "1",
                Permissions = new List<string>
        {
            "BRANCH_VIEW",
            "BRANCH_CREATE",
            "BRANCH_UPDATE",
            "BRANCH_STATUS"
        }
            };
        }
        // ========================
        // GET ALL
        // ========================
        [HttpPost("BranchList")]
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
        [HttpPost("BranchById")]
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
        [HttpPost("NewBranch")]
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
        [HttpPost("EditBranch")]
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
        [HttpPost("BranchStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus([FromBody] BranchRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(
                request.BranchId,
                request.Status ?? 0,
                uid,
                user
            );

            return Ok(result);
        }
    }
}