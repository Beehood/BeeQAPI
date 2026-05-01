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
    public class DAL_User : IDAL_User
    {
        private readonly DBConnection _config;

        public DAL_User(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL (Dynamic - Multi Result)
        // ========================
        /// <summary>
        /// User DAL - Get All Users
        /// Author: Swapnlisa
        /// Description:- Fetches paginated user list using stored procedure (multi-result).
        public async Task<APIGetResponseModel<List<UserModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<UserModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_UserId", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_Name", null);
                param.Add("p_Email", null);
                param.Add("p_Password", null);
                param.Add("p_RoleId", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserId_Login", null);

                using var multi = await conn.QueryMultipleAsync("sp_manage_user", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<UserModel>()).ToList();

                response.Result = list;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
                Console.WriteLine("DAL GET ALL ERROR: " + ex.ToString());
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// User DAL - Get User By Id
        /// Author: Swapnlisa
        /// Description:- Fetches user using UserId.
        public async Task<APIGetResponseModel<UserModel>> GetById(long id, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<UserModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_UserId", id);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_Name", null);
                param.Add("p_Email", null);
                param.Add("p_Password", null);
                param.Add("p_RoleId", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId_Login", null);

                var data = await conn.QueryFirstOrDefaultAsync<UserModel>("sp_manage_user", param, commandType: CommandType.StoredProcedure);

                if (data != null)
                {
                    response.Result = data;
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
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching user");
                Console.WriteLine("DAL GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        /// <summary>
        /// User DAL - Insert User
        /// Author: Swapnlisa
        /// Description:- Inserts new user using stored procedure.
        public async Task<APIGetResponseModel<long>> Insert(UserRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_UserId", null);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_Name", request.Name);
                param.Add("p_Email", request.Email);
                param.Add("p_Password", request.Password);
                param.Add("p_RoleId", request.RoleId);
                param.Add("p_Status", 1);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId_Login", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_user", param, commandType: CommandType.StoredProcedure);

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
                Console.WriteLine("DAL INSERT ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// User DAL - Update User
        /// Author: Swapnlisa
        /// Description:- Updates user details.
        public async Task<APIGetResponseModel<long>> Update(UserRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_UserId", request.UserId);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_Name", request.Name);
                param.Add("p_Email", request.Email);
                param.Add("p_Password", request.Password);
                param.Add("p_RoleId", request.RoleId);
                param.Add("p_Status", request.Status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId_Login", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_user", param, commandType: CommandType.StoredProcedure);

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
                Console.WriteLine("DAL UPDATE ERROR: " + ex.ToString());
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// User DAL - Change Status
        /// Author: Swapnlisa
        /// Description:- Updates user status.
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_UserId", id);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_Name", null);
                param.Add("p_Email", null);
                param.Add("p_Password", null);
                param.Add("p_RoleId", null);
                param.Add("p_Status", status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId_Login", userId);

                var result = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_user",
                    param,
                    commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = result > 0;
                response.TotalRecords = result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while changing status");
                Console.WriteLine("DAL STATUS ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        /// <summary>
        /// User DAL - Dropdown
        /// Author: Swapnlisa
        /// Description:- Fetches active users dropdown list.
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var data = (await conn.QueryAsync<DropdownModel>(
                    @"SELECT user_id AS Id, name AS Name FROM users WHERE status = 1"))
                    .ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = data.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching dropdown");
                Console.WriteLine("DAL DROPDOWN ERROR: " + ex.Message);
            }

            return response;
        }
    }
}

