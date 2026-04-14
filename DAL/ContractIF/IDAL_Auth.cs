using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Auth
    {
        Task<APIGetResponseModel<UserDetails>> ValidateUser(string username, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<UserProfileDetails>> loginprofile(string UserId, IDbTransaction? transaction = null);
    }
}
