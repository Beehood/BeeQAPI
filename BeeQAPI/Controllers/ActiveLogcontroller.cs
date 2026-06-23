using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
   
[Route("BeeQAPI")]
    [ApiController]
    public class ActivityLogController : ControllerBase
    {
        private readonly IBAL_ActiveLog _bal;

        public ActivityLogController(IBAL_ActiveLog bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================

        [Authorize]
        [HttpPost("ActiveLogList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<ActivityLogModel>>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<ActivityLogModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request,roles,email,transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================

        [Authorize]
        [HttpPost("ActiveLogById")]
        [ProducesResponseType(typeof(APIGetResponseModel<ActivityLogModel>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<ActivityLogModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id,roles,email,transaction: null);
        }

        // ========================
        // CREATE
        // ========================

        [Authorize]
        [HttpPost("NewActiveLog")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] ActivityLogRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request,roles,email,transaction: null);
        }
    }

}