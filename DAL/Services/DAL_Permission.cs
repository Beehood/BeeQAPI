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
    public class DAL_Permission : IDAL_Permission
    {
        private readonly DBConnection _config;

        public DAL_Permission(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL (Dynamic - Multi Result)
        // ========================
        /// <summary>
        /// Permission DAL - Get All Permissions
        /// Author: Swapnlisa
        /// Description:- Fetches paginated permission list using stored procedure.
        /// </summary>
        public async Task<APIGetResponseModel<List<PermissionModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<PermissionModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_PermissionId", null);
                param.Add("p_PermissionName", null);
                param.Add("p_PermissionCode", null);
                param.Add("p_Module", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserId", null);

                using var multi = await conn.QueryMultipleAsync(
                    "sp_manage_permission",
                    param,
                    commandType: CommandType.StoredProcedure);

                // 1st Result → Total Count
                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // 2nd Result → Data
                var list = (await multi.ReadAsync<PermissionModel>()).ToList();

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
        // INSERT
        // ========================
        /// <summary>
        /// Permission DAL - Insert Permission
        /// Author: Swapnlisa
        /// Description:- Inserts new permission record.
        /// </summary>
        public async Task<APIGetResponseModel<long>> Insert(PermissionRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_PermissionId", null);
                param.Add("p_PermissionName", request.PermissionName);
                param.Add("p_PermissionCode", request.PermissionCode);
                param.Add("p_Module", request.Module);
                param.Add("p_Status", 1);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", long.TryParse(userId, out var uid) ? uid : 0);

                var id = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_permission",
                    param,
                    commandType: CommandType.StoredProcedure);

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
        /// Permission DAL - Update Permission
        /// Author: Swapnlisa
        /// Description:- Updates existing permission details.
        /// </summary>
        public async Task<APIGetResponseModel<long>> Update(PermissionRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_PermissionId", request.PermissionId);
                param.Add("p_PermissionName", request.PermissionName);
                param.Add("p_PermissionCode", request.PermissionCode);
                param.Add("p_Module", request.Module);
                param.Add("p_Status", request.Status ?? 1);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_permission",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating permission");
                Console.WriteLine("DAL UPDATE ERROR: " + ex.Message);
            }

            return response;
        }
        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Permission DAL - Change Status
        /// Author: Swapnlisa
        /// Description:- Updates permission status (Active/Inactive).
        /// </summary>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_PermissionId", id);
                param.Add("p_PermissionName", null);
                param.Add("p_PermissionCode", null);
                param.Add("p_Module", null);
                param.Add("p_Status", status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var result = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_permission",
                    param,
                    commandType: CommandType.StoredProcedure
                );

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
        /// Permission DAL - Get Dropdown
        /// Author: Swapnlisa
        /// Description:- Fetches active permission dropdown list.
        /// </summary>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var data = (await conn.QueryAsync<DropdownModel>(
                    @"SELECT permission_id AS Id, permission_name AS Name 
                      FROM permissions 
                      WHERE status = 1"))
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