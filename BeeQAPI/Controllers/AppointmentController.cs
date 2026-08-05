using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers

{

    [Route("BeeQAPI")]

    [ApiController]

    public class AppointmentController : ControllerBase

    {

        private readonly IBAL_Appointment _bal;

        public AppointmentController(IBAL_Appointment bal)

        {

            _bal = bal;

        }

        // ========================

        // GET ALL

        // ========================

        [Authorize(Policy = "VIEW_APPOINTMENT")]

        [HttpPost("AppointmentList")]

        [ProducesResponseType(typeof(APIGetResponseModel<List<AppointmentModel>>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<List<AppointmentModel>>> GetAll([FromBody] AppointmentRequestDto request)

        {

            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetAll(request, roles, email, transaction: null);

        }

        // ========================

        // GET BY ID

        // ========================

        [Authorize(Policy = "VIEW_APPOINTMENT")]

        [HttpPost("AppointmentById")]

        [ProducesResponseType(typeof(APIGetResponseModel<AppointmentModel>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<AppointmentModel>> GetById([FromBody] long id)

        {

            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.GetById(id, roles, email, transaction: null);

        }

        // ========================

        // CREATE

        // ========================

        [Authorize(Policy = "CREATE_APPOINTMENT")]

        [HttpPost("NewAppointment")]

        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>> Create([FromBody] AppointmentRequestDto request)

        {

            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Create(request, roles, email, transaction: null);

        }

        // ========================

        // UPDATE

        // ========================

        [Authorize(Policy = "UPDATE_APPOINTMENT")]

        [HttpPost("EditAppointment")]

        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>> Update([FromBody] AppointmentRequestDto request)

        {

            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _bal.Update(request, roles, email, transaction: null);

        }

        // ========================

        // STATUS

        // ========================

        [Authorize(Policy = "DELETE_APPOINTMENT")]

        [HttpPost("AppointmentStatus")]

        [ProducesResponseType(typeof(APIGetResponseModel<int>), (int)HttpStatusCode.OK)]

        public async Task<APIGetResponseModel<int>> ChangeStatus([FromBody] AppointmentStatusRequestDto request)

        {

            var token = Request.Headers["Authorization"].ToString();

            foreach (var claim in User.Claims)

            {

                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");

            }

            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var branchId = User.FindFirst("BranchId")?.Value;

            return await _bal.ChangeStatus(request.AppointmentId, request.Status, roles, email, transaction: null);

        }

    }

}

