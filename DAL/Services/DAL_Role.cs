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
    public class DAL_Role : IDAL_Role
    {
        private readonly DBConnection _config;

        public DAL_Role(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL (Dynamic - Multi Result)
        // ========================
        /// <summary>
        /// Role DAL - Get All Roles
        /// Author: Swapnlisa
        /// Description:- Fetches paginated role list using stored procedure (multi-result).
        /// </summary>
        public async Task<APIGetResponseModel<List<RoleModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<RoleModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_RoleId", null);
                param.Add("p_RoleName", null);
                param.Add("p_RoleCode", null);
                param.Add("p_Description", null);
                param.Add("p_Status", null);
                param.Add("p_OrganizationId", null);

                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                //param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserId", null);

                using var multi = await conn.QueryMultipleAsync(
                    "sp_manage_role",
                    param,
                    commandType: CommandType.StoredProcedure);

                // 1st Result → Total Count
                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // 2nd Result → Data
                var list = (await multi.ReadAsync<RoleModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                //response.ErrorMsgs.Add("Error while fetching roles");
                response.ErrorMsgs.Add(ex.ToString());
                Console.WriteLine("DAL GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Role DAL - Get Role By Id
        /// </summary>
        public async Task<APIGetResponseModel<RoleModel>> GetById(long id, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<RoleModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_RoleId", id);
                param.Add("p_RoleName", null);
                param.Add("p_RoleCode", null);
                param.Add("p_Description", null);
                param.Add("p_Status", null);
                param.Add("p_OrganizationId", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", null);

                var data = await conn.QueryFirstOrDefaultAsync<RoleModel>(
                    "sp_manage_role",
                    param,
                    commandType: CommandType.StoredProcedure);

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
                response.ErrorMsgs.Add("Error while fetching role");
                Console.WriteLine("DAL GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        /// <summary>
        /// Role DAL - Insert Role
        /// </summary>
        public async Task<APIGetResponseModel<long>> Insert(RoleRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_RoleId", null);
                param.Add("p_RoleName", request.RoleName);
                param.Add("p_RoleCode", request.RoleCode);
                param.Add("p_Description", request.Description);
                param.Add("p_Status", 1);
                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_role",
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
        /// Role DAL - Update Role
        /// </summary>
        public async Task<APIGetResponseModel<long>> Update(RoleRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_RoleId", request.RoleId);
                param.Add("p_RoleName", request.RoleName);
                param.Add("p_RoleCode", request.RoleCode);
                param.Add("p_Description", request.Description);
                param.Add("p_Status", request.Status);
                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_role",
                    param,
                    commandType: CommandType.StoredProcedure);

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating role");
                Console.WriteLine("DAL UPDATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Role DAL - Change Status
        /// </summary>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_RoleId", id);
                param.Add("p_RoleName", null);
                param.Add("p_RoleCode", null);
                param.Add("p_Description", null);
                param.Add("p_Status", status);
                param.Add("p_OrganizationId", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var result = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_role",
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
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var data = (await conn.QueryAsync<DropdownModel>(
                    @"SELECT role_id AS Id, role_name AS Name FROM roles WHERE status = 1"))
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