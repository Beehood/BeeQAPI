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

        public async Task<APIGetResponseModel<ModelLoginResponse>> Login(
   LoginRequestDto dto,
   string salt,
   IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<ModelLoginResponse>
            {
                Result = new ModelLoginResponse()
            };


try
            {
                Console.WriteLine("TEST HASH: " + GetHashString("Admin"));
                // 🔹 Step 1: Validate input
                if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid login data");
                    return response;
                }

                // 🔹 Step 2: Get user from DB
                var userResponse = await _dal.ValidateUser(dto.Email, transaction);
                var user = userResponse.Result;

                if (!userResponse.IsSuccess || user == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid email or password");
                    return response;
                }

                // 🔥 DEBUG (IMPORTANT — REMOVE LATER)
                Console.WriteLine("====== LOGIN DEBUG ======");
                Console.WriteLine("DB PASSWORD (SHA512): " + user.Password);
                Console.WriteLine("SALT: " + salt);
                Console.WriteLine("DTO PASSWORD (FINAL HASH): " + dto.Password);

                // 🔥 Step 3: Validate password (CATERING STYLE)

                // DB contains: SHA512(password)
                string saltedInput = salt + (user.Password ?? "").Trim();

                string inputHash = GetHashString(saltedInput);

                Console.WriteLine("BACKEND GENERATED HASH: " + inputHash);
                Console.WriteLine("=========================");

                // 🔥 SAFE COMPARISON
                if (!string.Equals(inputHash, dto.Password, StringComparison.OrdinalIgnoreCase))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid password");
                    return response;
                }

                // 🔥 Step 4: Generate JWT (RBAC)
                var tokenUser = new TokenUserInfo
                {
                    Username = user.UserName,
                    Name = user.Name,
                    Roles = user.Roles,
                    Permissions = user.Permissions
                };

                string token = await _tokenService.GenerateTokenAsync(tokenUser);

                // 🔹 Step 5: Success response
                response.IsSuccess = true;
                response.Result.AuthToken = token;
                response.TotalRecords = 1;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Something went wrong");
                Console.WriteLine("LOGIN ERROR: " + ex.Message);
            }

            return response;


}


        public async Task<APIGetResponseModel<UserProfileDetails>> loginprofile(string UserId, IDbTransaction? transaction = null)
        {
            return await _dal.loginprofile(UserId, transaction: transaction);
        }

        // 🔥 SHA512 HASH FUNCTION
        public static string GetHashString(string inputString)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        // 🔥 RANDOM SALT (same as your existing)
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
