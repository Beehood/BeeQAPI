using BAL.ContractIF;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;

namespace BeeQAPI.Controllers
{
    [Route("Counter")]
    [ApiController]
    //[Authorize]
    public class CounterController : ControllerBase
    {
        private readonly IBAL_Counter _bal;

        public CounterController(IBAL_Counter bal)
        {
            _bal = bal;
        }

        // 🔥 Get user from middleware
        //private TokenUserInfo GetUser()
        //{
        //    return HttpContext.Items["User"] as TokenUserInfo;
        //}

        //for temporaly testing without auth//
        private TokenUserInfo GetUser()
        {
            return new TokenUserInfo
            {
                Username = "1",
                Permissions = new List<string>
                {
                    "COUNTER_VIEW",
                    "COUNTER_CREATE",
                    "COUNTER_UPDATE",
                    "COUNTER_STATUS"
                }
            };
        }

        // ========================
        // GET ALL
        // ========================
        [HttpPost("CounterList")]
        [ProducesResponseType(typeof(APIGetResponseModel<List<CounterModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.GetAll(request, user);
            return Ok(result);
        }

        // ========================
        // GET BY ID
        // ========================
        [HttpPost("CounterById")]
        [ProducesResponseType(typeof(APIGetResponseModel<CounterModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromBody] long id)
        {
            var user = GetUser();

            var result = await _bal.GetById(id, user);
            return Ok(result);
        }

        // ========================
        // CREATE
        // ========================
        [HttpPost("NewCounter")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CounterRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Create(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // UPDATE
        // ========================
        [HttpPost("EditCounter")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] CounterRequestDto request)
        {
            var user = GetUser();

            var result = await _bal.Update(request, user.Username, user);
            return Ok(result);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [HttpPost("CounterStatus")]
        [ProducesResponseType(typeof(APIGetResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus([FromBody] CounterRequestDto request)
        {
            var user = GetUser();

            long uid = long.TryParse(user.Username, out var parsed) ? parsed : 0;

            var result = await _bal.ChangeStatus(
                request.CounterId,
                request.Status,
                uid,
                user
            );

            return Ok(result);
        }
    }
}
