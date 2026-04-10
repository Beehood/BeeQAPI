using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BeeQAPI.Controllers
{
    [Route("BeeQAPI")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IBAL_Auth _auth;
        static string salt = "";

        public AuthController(IBAL_Auth auth)
        {
            _auth = auth;
        }

        [HttpGet("GetSalt")]
        public async Task<string> GetSalt()
        {
            salt = await _auth.RandomString();
            return salt;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(APIGetResponseModel<ModelLoginResponse>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<ModelLoginResponse>> Login([FromBody] LoginRequestDto dto)
        {
            return await _auth.Login(dto, salt, transaction: null);
        }

        [Authorize]
        [HttpPost("loginprofile")]
        [ProducesResponseType(typeof(APIGetResponseModel<UserProfileDetails>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<UserProfileDetails>> loginprofile()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _auth.loginprofile(userId, transaction: null);
        }
    }
}
