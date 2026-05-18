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
    public class TokenController : ControllerBase
    {
        private readonly IBAL_Token _bal;

        public TokenController(IBAL_Token bal)
        {
            _bal = bal;
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
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> GenerateToken([FromBody] TokenRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GenerateToken(request, roles, email, transaction: null);
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
    }

}
