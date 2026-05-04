using BAL.ContractIF.BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;


namespace BeeQAPI.Controllers
{
    [Route("branchservice")]
    [ApiController]
    //[Authorize]
    public class BranchServiceController : ControllerBase
    {
        private readonly IBAL_BranchService _bal;

        public BranchServiceController(IBAL_BranchService bal)
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
                    "BRANCHSERVICE_VIEW",
                    "BRANCHSERVICE_CREATE",
                    "BRANCHSERVICE_UPDATE",
                    "BRANCHSERVICE_STATUS"
                }
            };
        }

        // ========================
        // GET ALL
        // ========================
        //[Authorize(Policy = "BRANCHSERVICE_VIEW")]
        [HttpPost("BranchServiceList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<BranchServiceModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.GetAll(request, user);
            return Ok(result);
        }

        // ========================
        // GET BY ID
        // ========================
        //[Authorize(Policy = "BRANCHSERVICE_VIEW")]
        [HttpPost("BranchServiceById")]
        [ProducesResponseType(typeof(APIGetResponseModel<BranchServiceModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromBody] long id)
        {
            var user = GetUser(); // 🔁 replace with JWT later

            var result = await _bal.GetById(id, user);
            return Ok(result);
        }

        // ========================
        // CREATE
        // ========================
        //[Authorize(Policy = "BRANCHSERVICE_CREATE")]
        [HttpPost("NewBranchService")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] BranchServiceRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Create(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // UPDATE
        // ========================
        //[Authorize(Policy = "BRANCHSERVICE_UPDATE")]
        [HttpPost("EditBranchService")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] BranchServiceRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Update(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        //[Authorize(Policy = "BRANCHSERVICE_STATUS")]
        [HttpPost("BranchServiceStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus([FromBody] BranchServiceRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(
                request.BranchServiceId,
                request.Status == true ? 1 : 0,
                uid,
                user
            );

            return Ok(result);
        }
        // ===================
        // DROPDOWN (PRODUCTION READY)
        // ===================
        [HttpGet("BranchServiceDropdown")]
        //[Authorize(Policy = "BRANCHSERVICE_VIEW")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), 200)]
        public async Task<IActionResult> GetDropdown()
        {
            var user = HttpContext.Items["User"] as TokenUserInfo;

            var result = await _bal.GetDropdown(user);
            return Ok(result);
        }
    }
}