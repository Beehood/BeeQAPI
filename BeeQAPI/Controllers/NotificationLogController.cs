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
    public class NotificationLogController : ControllerBase
    {
        private readonly IBAL_NotificationLog _bal;

        public NotificationLogController(IBAL_NotificationLog bal)
        {
            _bal = bal;
        }

        // ========================
        // GET ALL
        // ========================
        //[Authorize(Policy = "VIEW_NOTIFICATION_LOG")]
        [HttpPost("NotificationLogList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<NotificationLogModel>>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<List<NotificationLogModel>>> GetAll([FromBody] PaginationRequestDto request)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);
        }

        // ========================
        // GET BY ID
        // ========================
        [Authorize(Policy = "VIEW_NOTIFICATION_LOG")]
        [HttpPost("NotificationLogById")]
        [ProducesResponseType(typeof(APIGetResponseModel<NotificationLogModel>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<NotificationLogModel>> GetById([FromBody] long id)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);
        }

        // ========================
        // CREATE
        // ========================
        [Authorize(Policy = "CREATE_NOTIFICATION_LOG")]
        [HttpPost("NewNotificationLog")]
        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<int>> Create([FromBody] NotificationLogRequestDto request)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);
        }
    }
}