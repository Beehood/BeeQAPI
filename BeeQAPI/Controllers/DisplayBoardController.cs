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
    public class DisplayBoardController : ControllerBase
    {
        private readonly IBAL_DisplayBoard _bal;

        public DisplayBoardController(IBAL_DisplayBoard bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_DISPLAY")]
        [HttpPost("DisplayBoardList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DisplayBoardModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DisplayBoardModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================
        [Authorize(Policy = "VIEW_DISPLAY")]
        [HttpPost("DisplayBoardById")]
        [ProducesResponseType(typeof(APIGetResponseModel<DisplayBoardModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<DisplayBoardModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE
        // ========================
        [Authorize(Policy = "CREATE_DISPLAY")]
        [HttpPost("NewDisplayBoard")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] DisplayBoardRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // UPDATE
        // ========================
        [Authorize(Policy = "UPDATE_DISPLAY")]
        [HttpPost("EditDisplayBoard")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Update([FromBody] DisplayBoardRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);
        }

        // ========================
        // STATUS (DELETE / ACTIVATE)
        // ========================
        [Authorize(Policy = "DELETE_DISPLAY")]
        [HttpPost("DisplayBoardStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] DisplayBoardRequestDto request)
        {
            Console.WriteLine("=== DISPLAY BOARD CONTROLLER START ===");

            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine("Roles: " + string.Join(",", roles));
            Console.WriteLine("Email: " + email);
            Console.WriteLine("DisplayId: " + request.DisplayId);

            return await _bal.ChangeStatus(request.DisplayId, roles, email, transaction: null);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_DISPLAY")]
        [HttpGet("DisplayBoardDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDropdown(email, transaction: null);
        }
    }
}