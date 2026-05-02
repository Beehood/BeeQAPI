using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
    [Route("organizations")]
    [ApiController]
    //[Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IBAL_Organization _bal;

        public OrganizationController(IBAL_Organization bal)
        {
            _bal = bal;
        }

        // 🔥 Get user from middleware
        //private TokenUserInfo GetUser()
        //{
        //    return HttpContext.Items["User"] as TokenUserInfo;
        //}

        private TokenUserInfo GetUser()
        {
            return new TokenUserInfo
            {
                Username = "1",
                Permissions = new List<string>
        {                                                      //for temporaly testing without auth
            "VIEW_ORG",
            "CREATE_ORG",
            "UPDATE_ORG",
            "DELETE_ORG"
        }
            };

        }
        //private TokenUserInfo GetUser()
        //{
        //    var user = new TokenUserInfo();

        //    user.Username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    user.Permissions = User.Claims
        //        .Where(c => c.Type == "permission")
        //        .Select(c => c.Value)
        //        .ToList();

        //    return user;
        //}

        // ========================
        // GET ALL
        // ========================
       
        [HttpPost("OrganizationList")]
        //[Authorize(Policy = "VIEW_ORG")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<OrganizationModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.GetAll(request, user);
            return Ok(result);
        }

        // ========================
        // GET BY ID
        // ========================
        [HttpPost("OrganizationById")]
        //[Authorize(Policy = "VIEW_ORG")]
        [ProducesResponseType(typeof(APIGetResponseModel<OrganizationModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromBody] long id)
        {
            var user = GetUser();

            var result = await _bal.GetById(id, user);
            return Ok(result);
        }

        // ========================
        // CREATE
        // ========================
        [HttpPost("NewOrganization")]
        //[Authorize(Policy = "CREATE_ORG")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] OrganizationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Create(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // UPDATE
        // ========================
        [HttpPost("EditOrganization")]
        //[Authorize(Policy = "UPDATE_ORG")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] OrganizationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Update(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [HttpPost("OrganizationStatus")]
        //[Authorize(Policy = "DELETE_ORG")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus([FromBody] OrganizationRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(request.OrganizationId ,request.Status,uid,user);

            return Ok(result);
        }
        //// ===================
        //// DROPDOWN
        //// ===================
        //[HttpGet("OrganizationDropdown")]
        ////[Authorize(Policy = "ORG_VIEW")]
        //[ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), 200)]
        //public async Task<IActionResult> GetDropdown()
        //{
        //    var user = HttpContext.Items["User"] as TokenUserInfo;

        //    var result = await _bal.GetDropdown(user);
        //    return Ok(result);
        //}
    }
}