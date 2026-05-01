using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class DAL_Auth : IDAL_Auth
    {
        private readonly DBConnection _config;
        public DAL_Auth(DBConnection config)
        {
            _config = config;
        }
        // =========================
        // VALIDATE USER (LOGIN)
        // =========================
        public async Task<APIGetResponseModel<UserDetails>> ValidateUser(string username, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<UserDetails>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Email", username);

                // 🔥 FIX: Use QueryFirst instead of QueryMultiple
                var user = await conn.QueryFirstOrDefaultAsync<UserDetails>(
                    "sp_User_Login",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (user != null)
                {
                    response.Result = user;
                    response.TotalRecords = 1;
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.TotalRecords = 0;
                    response.Result = null;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.TotalRecords = 0;
                response.Result = null;
                response.ErrorMsgs.Add("Error while validating user");
            }

            return response;
        }

        // =========================
        // LOGIN PROFILE
        // =========================
        public async Task<APIGetResponseModel<UserProfileDetails>> loginprofile(string UserId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<UserProfileDetails>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("P_Action", "LOGINPROFILE");
                param.Add("p_UserId", UserId);
               

                using var multi = await conn.QueryMultipleAsync(
                    "sp_User",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                var profile =
                    await multi.ReadFirstOrDefaultAsync<UserProfileDetails>();

                if (profile != null)
                {
                    profile.Roles = (await multi.ReadAsync<string>()).ToList();
                    profile.Permissions = (await multi.ReadAsync<string>()).ToList();


                    response.Result = profile;
                    response.TotalRecords = 1;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = null;
                    response.TotalRecords = 0;
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.TotalRecords = 0;
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }
    }
}
