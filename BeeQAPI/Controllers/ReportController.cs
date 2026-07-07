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

    public class ReportController : ControllerBase

    {

        private readonly IBAL_Report _bal;

        public ReportController(IBAL_Report bal)

        {

            _bal = bal;

        }

        // ========================

        // REPORT LIST

        // ========================

        [Authorize(Policy = "VIEW_REPORTS")]

        [HttpPost("ReportList")]

        [ProducesResponseType(typeof(APIGetResponseModel<List<ReportModel>>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<List<ReportModel>>> GetAll([FromBody] ReportRequestDto request)

        {

            Console.WriteLine($"CONTROLLER ACTION = {request.Action}");

            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);

        }

    }

}
