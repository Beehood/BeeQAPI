using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace BAL.Services
{
    public class BAL_Auth : IBAL_Auth
    {
        private readonly IDAL_Auth _dal;
        private readonly IJwtService _tokenService;
        public BAL_Auth(IDAL_Auth dal, IJwtService tokenService)
        {
            _dal = dal;
            _tokenService = tokenService;
        }
        public async Task<APIGetResponseModel<ModelLoginResponse>> Login(LoginRequestDto dto,string salt,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<ModelLoginResponse>
            {
                Result = new ModelLoginResponse()
            };

            try
            {
                // Step 1: Validate input
                if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid login data");
                    return response;
                }

                //  Step 2: Get user from DB
                var userResponse = await _dal.ValidateUser(dto.Email, transaction);
                var user = userResponse.Result;

                if (!userResponse.IsSuccess || user == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid email or password");
                    return response;
                }

                //Step 3: DB contains: SHA512(password) 
                string saltedInput = salt + user.Password;
                string inputHash = GetHashString(saltedInput);

                if (inputHash != dto.Password)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid password");
                    return response;
                }

                // Step 4: Generate JWT (RBAC)
                var tokenUser = new TokenUserInfo
                {
                    Username = user.UserName,
                    Name = user.Name,
                    OrganizationId = user.OrganizationId,
                    BranchId = user.BranchId,
                    CounterId = user.CounterId,
                    Roles = user.Roles ?? new List<string>(),
                    Permissions = user.Permissions ?? new List<string>()
                };

                string token = await _tokenService.GenerateTokenAsync(tokenUser);

                // Step 5: Success response
                response.IsSuccess = true;
                response.Result.AuthToken = token;
                response.TotalRecords = 1;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
               
            }
            return response;
        }

        public async Task<APIGetResponseModel<UserProfileDetails>> loginprofile(string UserId, IDbTransaction? transaction = null)
        {
            return await _dal.loginprofile(UserId, transaction: transaction);
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public async Task<string> RandomString()
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] result = new char[32];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[32];
                rng.GetBytes(data);

                for (int i = 0; i < result.Length; i++)
                {
                    int index = data[i] % chars.Length;
                    result[i] = chars[index];
                }
            }
            return new string(result);
        }
    }
}
