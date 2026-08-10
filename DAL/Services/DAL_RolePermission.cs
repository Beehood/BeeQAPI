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

    public class DAL_RolePermission : IDAL_RolePermission

    {
        private readonly DBConnection _config;
        public DAL_RolePermission(DBConnection config)

        {
            _config = config;
        }
        // ========================
        // GET ALL
        // ========================

        /// <summary>

        /// RolePermission DAL - Get All Role Permissions

        /// Author: Swapnlisa

        /// Description:- Fetches paginated role-permission mapping list using stored procedure.

        /// </summary>

        public async Task<APIGetResponseModel<List<RolePermissionModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

        {
            var response = new APIGetResponseModel<List<RolePermissionModel>>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETALL");

                param.Add("p_RoleId", null);

                param.Add("p_PermissionId", null);

                param.Add("p_PermissionIds", null);

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_role_permission", param, commandType: CommandType.StoredProcedure);

                // FIRST RESULT SET

                var list = (await multi.ReadAsync<RolePermissionModel>()).ToList();

                // SECOND RESULT SET

                response.TotalRecords = (await multi.ReadAsync<int>()).FirstOrDefault();

                response.Result = list;

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);

                Console.WriteLine("DAL ROLE PERMISSION ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================
        // GET BY ROLE
        // ========================

        /// <summary>

        /// RolePermission DAL - Get Permissions By RoleId

        /// </summary>

        public async Task<APIGetResponseModel<List<RolePermissionModel>>> GetByRoleId(long roleId, string email, IDbTransaction? transaction = null)

        {
            var response = new APIGetResponseModel<List<RolePermissionModel>>();
            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYROLE");

                param.Add("p_RoleId", roleId);

                param.Add("p_PermissionId", null);

                param.Add("p_PermissionIds", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_PageSize", null);

                param.Add("p_UserEmail", email);

                string sql = "CALL sp_manage_role_permission(@p_Action, @p_RoleId, @p_PermissionId, @p_PermissionIds, @p_SearchKey, @p_PageNo, @p_PageSize, @p_UserEmail);";

                var data = (await conn.QueryAsync<RolePermissionModel>(sql, param, commandType: CommandType.Text)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = data.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);

                Console.WriteLine("DAL ROLE PERMISSION GET BY ROLE ERROR: " + ex.ToString());

            }

            return response;

        }



        // ========================

        // INSERT (Single)

        // ========================

        /// <summary>

        /// RolePermission DAL - Insert Single Role Permission

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(RolePermissionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_RoleId", request.RoleId);

                param.Add("p_PermissionId", request.PermissionId);

                param.Add("p_PermissionIds", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_PageSize", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_role_permission", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting role permission");

                Console.WriteLine("DAL ROLE PERMISSION INSERT ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // BULK INSERT ( UPDATE Equivalent)

        // ========================

        /// <summary>

        /// RolePermission DAL - Bulk Assign Permissions to Role

        /// </summary>

        public async Task<APIGetResponseModel<int>> BulkInsert(RolePermissionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "BULKASSIGN");

                param.Add("p_RoleId", request.RoleId);

                param.Add("p_PermissionId", null);

                param.Add("p_PermissionIds", request.PermissionIds);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_PageSize", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_role_permission", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while bulk assigning permissions");

                Console.WriteLine("DAL ROLE PERMISSION BULK INSERT ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // DELETE

        // ========================

        /// <summary>

        /// RolePermission DAL - Delete Role Permission

        /// </summary>

        public async Task<APIGetResponseModel<int>> Delete(RolePermissionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "REMOVE");

                param.Add("p_RoleId", request.RoleId);

                param.Add("p_PermissionId", request.PermissionId);

                param.Add("p_PermissionIds", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_PageSize", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_role_permission", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while deleting role permission");

            }

            return response;

        }

    }

}

