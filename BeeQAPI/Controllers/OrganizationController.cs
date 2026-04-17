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
        private readonly IOrganizationBAL _bal;

        public OrganizationController(IOrganizationBAL bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL (Admin + Manager)
        // ========================
        //[Authorize(Roles = "Admin,Manager")]
        [AllowAnonymous]
        [HttpPost("GetAll")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<OrganizationModel>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<APIGetResponseModel<List<OrganizationModel>>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var result = await _bal.GetAll(request, transaction: null);
            return Ok(result);
        }

        // ========================
        //GET BY ID(All roles)
        // ========================
        //[Authorize(Roles = "Admin,Manager,User")]
        [AllowAnonymous]
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(APIGetResponseModel<OrganizationModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<APIGetResponseModel<OrganizationModel>>> GetById([FromBody] long id)
        {
            var result = await _bal.GetById(id, transaction: null);
            return Ok(result);
        }

        // ========================
        // CREATE (Admin only)
        // ========================
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<APIGetResponseModel<long>>> Create([FromBody] OrganizationRequestDto request)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(userId, out long uid))
            {
                request.UserId = uid;
            }

            var result = await _bal.Create(request, transaction: null);
            return Ok(result);
        }

        // ========================
        // UPDATE (Admin + Manager)
        // ========================
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("Update")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<APIGetResponseModel<long>>> Update([FromBody] OrganizationRequestDto request)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(userId, out long uid))
            {
                request.UserId = uid;
            }

            var result = await _bal.Update(request, transaction: null);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS (Admin only)
        // ========================
        [Authorize(Roles = "Admin")]
        [HttpPost("ChangeStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<APIGetResponseModel<long>>> ChangeStatus([FromBody] OrganizationRequestDto request)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            long uid = 0;
            if (long.TryParse(userId, out long parsed))
            {
                uid = parsed;
            }

            var result = await _bal.ChangeStatus(
                request.OrganizationId ?? 0,
                request.Status ?? 0,
                uid,
                transaction: null
            );

            return Ok(result);
        }
    }
}
