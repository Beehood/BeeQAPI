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
                param.Add("p_Action", "LOGIN");
                param.Add("p_UserId", username);

                using var multi = await conn.QueryMultipleAsync("sp_User",param,commandType: CommandType.StoredProcedure);

                //USER details
                var user = await multi.ReadFirstOrDefaultAsync<UserDetails>();

                if (user == null)
                {
                    response.IsSuccess = false;
                    response.TotalRecords = 0;
                    response.Result = null;
                    return response;
                }

                //ROLES
                var roles = (await multi.ReadAsync<string>()).ToList();

                //PERMISSIONS
                var permissions = (await multi.ReadAsync<string>()).ToList();

                // Assign to model
                user.Roles = roles;
                user.Permissions = permissions;

                response.Result = user;
                response.TotalRecords = 1;
                response.IsSuccess = true;
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
        public async Task<APIGetResponseModel<UserProfileDetails>> loginprofile(string username, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<UserProfileDetails>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "LOGINPROFILE");
                param.Add("p_UserId", username);

                // ✅ Only ONE result set now
                var profile = await conn.QueryFirstOrDefaultAsync<UserProfileDetails>("sp_User",param,commandType: CommandType.StoredProcedure);

                if (profile != null)
                {
                    // Optional safety fallback (in case DB returns null)
                    profile.ProfilPic = string.IsNullOrEmpty(profile.ProfilPic)
                        ? "default.png"
                        : profile.ProfilPic;

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
