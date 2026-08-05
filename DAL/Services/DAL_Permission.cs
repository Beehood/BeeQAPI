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

    /// <summary>

    /// Permission DAL Service

    /// Author: Swapnlisa

    /// Description:- Handles all database operations related to Permissions 

    /// using stored procedure (sp_manage_permission).

    /// Supports RBAC-based access via UserEmail.

    /// </summary>

    public class DAL_Permission : IDAL_Permission
    {
        private readonly DBConnection _config;
        public DAL_Permission(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================

        /// <summary>

        /// Fetches paginated list of permissions.

        /// Applies search filter and RBAC access based on logged-in user.

        /// </summary>

        /// <param name="request">PaginationRequestDto (SearchKey, PageNo)</param>

        /// <param name="email">Logged-in user email</param>

        /// <param name="transaction">Optional DB transaction</param>

        /// <returns>List of PermissionModel with total count</returns>

        public async Task<APIGetResponseModel<List<PermissionModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<PermissionModel>>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_PermissionId", null);

                param.Add("p_Name", null);

                param.Add("p_Code", null);

                param.Add("p_Module", null);
                param.Add("p_PermissionScope", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_permission", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<PermissionModel>()).ToList();

                response.Result = list;

                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                //response.ErrorMsgs.Add("Error while fetching permissions");
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;

        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// Fetches permission details by PermissionId.

        /// </summary>

        /// <param name="id">PermissionId</param>

        /// <param name="email">Logged-in user email</param>

        /// <param name="transaction">Optional DB transaction</param>

        /// <returns>Single PermissionModel</returns>

        public async Task<APIGetResponseModel<PermissionModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<PermissionModel>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_PermissionId", id);

                param.Add("p_Name", null);

                param.Add("p_Code", null);

                param.Add("p_Module", null);
                param.Add("p_PermissionScope",null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<PermissionModel>("sp_manage_permission", param, commandType: CommandType.StoredProcedure);

                if (data != null)

                {

                    response.Result = data;

                    response.TotalRecords = 1;

                    response.IsSuccess = true;

                }

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching permission");

                Console.WriteLine("DAL PERMISSION GET BY ID ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// Inserts a new permission record.

        /// Validates uniqueness of permission code in stored procedure.

        /// </summary>

        /// <param name="request">PermissionRequestDto</param>

        /// <param name="email">Logged-in user email</param>

        /// <param name="transaction">Optional DB transaction</param>

        /// <returns>Newly created PermissionId</returns>

        public async Task<APIGetResponseModel<int>> Insert(PermissionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_PermissionId", null);

                param.Add("p_Name", request.PermissionName);

                param.Add("p_Code", request.PermissionCode);

                param.Add("p_Module", null);
                param.Add("p_PermissionScope", request.PermissionScope);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_permission", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting permission");

                Console.WriteLine("DAL PERMISSION INSERT ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // UPDATE

        // ========================

        /// <summary>

        /// Updates existing permission details.

        /// Ensures unique permission code validation.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Update(PermissionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_PermissionId", request.PermissionId);

                param.Add("p_Name", request.PermissionName);

                param.Add("p_Code", request.PermissionCode);

                param.Add("p_Module", request.Module);
                param.Add("p_PermissionScope", request.PermissionScope);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_permission", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);

                Console.WriteLine("DAL PERMISSION UPDATE ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // CHANGE STATUS

        // ========================

        /// <summary>

        /// Toggles permission status (Active/Inactive).

        /// </summary>

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_PermissionId", id);

                param.Add("p_Name", null);

                param.Add("p_Code", null);

                param.Add("p_Module", null);
                param.Add("p_PermissionScope", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_permission", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while changing permission status");

                Console.WriteLine("DAL PERMISSION STATUS ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // DROPDOWN

        // ========================

        /// <summary>

        /// Fetches active permissions for dropdown.

        /// Used in UI for selection lists.

        /// </summary>

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<DropdownModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");

                param.Add("p_PermissionId", null);

                param.Add("p_Name", null);

                param.Add("p_Code", null);

                param.Add("p_Module", null);
                param.Add("p_PermissionScope", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_permission", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = data.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching permission dropdown");

                Console.WriteLine("DAL PERMISSION DROPDOWN ERROR: " + ex.Message);

            }

            return response;

        }

    }

}
