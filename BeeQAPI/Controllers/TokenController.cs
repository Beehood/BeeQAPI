using BAL.ContractIF;
using BeeQAPI.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Net;
using System.Security.Claims;
namespace BeeQAPI.Controllers
{
    [Route("BeeQAPI")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IBAL_Token _bal;
        private readonly IHubContext<QueueHub> _hubContext;
        public TokenController(IBAL_Token bal, IHubContext<QueueHub> hubContext)
        {
            _bal = bal;
            _hubContext = hubContext;
        }
        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_TOKEN")]
        [HttpPost("TokenList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<TokenModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<TokenModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.GetAll(request, roles, email, transaction: null);
        }
        // ========================
        // GET BY ID
        // ========================
        [Authorize(Policy = "VIEW_TOKEN")]
        [HttpPost("TokenById")]
        [ProducesResponseType(typeof(APIGetResponseModel<TokenModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<TokenModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.GetById(id, roles, email, transaction: null);
        }
        // ========================
        // GENERATE TOKEN
        // ========================
        [Authorize(Policy = "CREATE_TOKEN")]
        [HttpPost("GenerateToken")]
        [ProducesResponseType(typeof(APIGetResponseModel<string>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<string>> GenerateToken([FromBody] TokenRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _bal.GenerateToken(request, roles, email, transaction: null);
            if (response.IsSuccess)
            {
                var branchId = await _bal.GetBranchIdByEmail(email);
                await _hubContext.Clients.Group(branchId.ToString()).SendAsync("QueueUpdated");
            }
            return response;
        }
        // ========================
        // CHANGE STATUS
        // ========================
        [Authorize(Policy = "UPDATE_TOKEN")]
        [HttpPost("TokenStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] TokenRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.ChangeStatus(request, roles, email, transaction: null);
        }
        // ========================
        // CALL NEXT TOKEN
        // ========================
        [Authorize(Policy = "UPDATE_TOKEN")]
        [HttpPost("CallNextToken")]
        [ProducesResponseType(typeof(APIGetResponseModel<TokenModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<TokenModel>> CallNextToken([FromBody] TokenRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.CallNextToken(request, roles, email, transaction: null);
        }
        // ========================
        // STATUS LIST
        // ========================
        [Authorize(Policy = "VIEW_TOKEN")]
        [HttpGet("TokenStatusList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<TokenStatusModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<TokenStatusModel>>> GetStatuses()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.GetStatuses(email, transaction: null);
        }
        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_TOKEN")]
        [HttpGet("TokenDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.GetDropdown(email, transaction: null);
        }

        [Authorize(Policy = "CREATE_TOKEN")]
        [HttpPost("NextTokenPreview")]
        [ProducesResponseType(typeof(APIGetResponseModel<TokenModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<TokenModel>> NextTokenPreview([FromBody] TokenRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _bal.NextTokenPreview(request, roles, email, transaction: null);
        }
    }
}

