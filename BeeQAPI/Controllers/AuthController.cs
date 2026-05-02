using BAL.ContractIF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
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

        [AllowAnonymous]
        [HttpGet("GetSalt")]
        public async Task<string> GetSalt()
        {
            salt = await _auth.RandomString();
            return salt;
        }

        //[AllowAnonymous]
        //[HttpGet("GetSalt")]
        //public async Task<string> GetSalt()
        //{
        //    return "fHwPLKwfFihBbjQ9QhP85yAylaEbtqXV"; // ✅ static salt
        //}


        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(APIGetResponseModel<ModelLoginResponse>), (int)HttpStatusCode.OK)]
        public async Task<APIGetResponseModel<ModelLoginResponse>> Login([FromBody] LoginRequestDto dto)
        {
            //string salt = "fHwPLKwfFihBbjQ9QhP85yAylaEbtqXV";
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
