using BAL.ContractIF;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BeeQAPI.Controllers
{
    [Route("counterservice")]
    [ApiController]
    //[Authorize]
    public class CounterServiceController : ControllerBase
    {
        private readonly IBAL_CounterService _bal;

        public CounterServiceController(IBAL_CounterService bal)
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
                    "COUNTERSERVICE_VIEW",
                    "COUNTERSERVICE_CREATE",
                    "COUNTERSERVICE_UPDATE",
                    "COUNTERSERVICE_STATUS"
                }
            };
        }

        // ========================
        // GET ALL
        // ========================
        [HttpPost("CounterServiceList")]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.GetAll(request, user);
            return Ok(result);
        }

        // ========================
        // GET BY ID
        // ========================
        [HttpPost("CounterServiceById")]
        public async Task<IActionResult> GetById([FromBody] long id)
        {
            var user = GetUser();

            var result = await _bal.GetById(id, user);
            return Ok(result);
        }

        // ========================
        // CREATE
        // ========================
        [HttpPost("NewCounterService")]
        public async Task<IActionResult> Create([FromBody] CounterServiceRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Create(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // UPDATE
        // ========================
        [HttpPost("EditCounterService")]
        public async Task<IActionResult> Update([FromBody] CounterServiceRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Update(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [HttpPost("CounterServiceStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] CounterServiceRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(
                request.CounterServiceId,
                request.Status ?? 0,
                uid,
                user
            );

            return Ok(result);
        }
        // =========================
        // COUNTERSERVICEDROPDOWN
        // =========================
        [HttpPost("CounterServiceDropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            var user = GetUser();
            var result = await _bal.GetDropdown(user);
            return Ok(result);
        }
    }
}