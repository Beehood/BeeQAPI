using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_Auth
    {
        Task<string> RandomString();
        Task<APIGetResponseModel<ModelLoginResponse>> Login(LoginRequestDto dto, string salt, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<UserProfileDetails>> loginprofile(string UserId, IDbTransaction? transaction = null);
    }
}
