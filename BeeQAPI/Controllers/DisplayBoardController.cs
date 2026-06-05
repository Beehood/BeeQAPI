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
        // DISPLAY BOARD (PUBLIC TV)
        // ========================
        //[Authorize(Policy = "VIEW_DISPLAY_BOARD")]
        [Authorize]
        //[HttpGet("DisplayBoardView")]
        //[ProducesResponseType(typeof(APIGetResponseModel<List<QueueDisplayModel>>), (int)HttpStatusCode.OK)]
        //public async Task<APIGetResponseModel<List<QueueDisplayModel>>> DisplayBoardView()
        //{
            
        //        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        return await _bal.GetDisplayData(username);
           
        //}

        [HttpPost("DisplayBoardView")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<QueueDisplayModel>>),(int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<QueueDisplayModel>>> DisplayBoardView()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDisplayData(username);
        }


      



        // ========================
        // GET ALL
        // ========================
        [Authorize(Policy = "VIEW_DISPLAY_BOARD")]
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
        [Authorize(Policy = "VIEW_DISPLAY_BOARD")]
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
        [Authorize(Policy = "CREATE_DISPLAY_BOARD")]
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
        [Authorize(Policy = "UPDATE_DISPLAY_BOARD")]
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
        [Authorize(Policy = "DELETE_DISPLAY_BOARD")]
        [HttpPost("DisplayBoardStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] DisplayBoardRequestDto request)
        {

            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.ChangeStatus(request.DisplayId, roles, email, transaction: null);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Authorize(Policy = "VIEW_DISPLAY_BOARD")]
        [HttpGet("DisplayBoardDropdown")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DropdownModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetDropdown(email, transaction: null);
        }
    }
}