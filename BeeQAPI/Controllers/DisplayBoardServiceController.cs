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
    public class DisplayBoardServiceController : ControllerBase
    {
        private readonly IBAL_DisplayBoardService _bal;

        public DisplayBoardServiceController(IBAL_DisplayBoardService bal)
        {
            _bal = bal;
        }

        // ========================
        // GET SERVICES BY DISPLAY
        // ========================
        [Authorize(Policy = "VIEW_DISPLAY_BOARD_SERVICE")]
        [HttpGet("DisplayBoardServiceList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<DisplayBoardServiceModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<DisplayBoardServiceModel>>> GetAll([FromQuery] long displayId)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(displayId, roles, email, transaction: null);
        }

        // ========================
        // CREATE (MAP SERVICE)
        // ========================
        [Authorize(Policy = "CREATE_DISPLAY_BOARD_SERVICE")]
        [HttpPost("AddDisplayBoardService")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] DisplayBoardServiceRequestDto request)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }

        // ========================
        // DELETE (REMOVE MAPPING)
        // ========================
        [Authorize(Policy = "DELETE_DISPLAY_BOARD_SERVICE")]
        [HttpPost("DeleteDisplayBoardService")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Delete([FromBody] long id)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Delete(id, roles, email, transaction: null);
        }
    }
}
