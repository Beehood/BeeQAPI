using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
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

        // GET ALL

        // ========================

        /// <summary>

        /// Role DAL - Get All Roles

        /// Description:- Retrieves all role records from the database with pagination and search functionality.

        /// </summary>

        public async Task<APIGetResponseModel<List<RoleModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

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

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_OrganizationId", null);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_role", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<RoleModel>()).ToList();

                response.Result = list;

                response.IsSuccess = list.Any();
            }

            catch (Exception ex)

            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching roles");

                Console.WriteLine("DAL ROLE GET ALL ERROR: " + ex.Message);

            }
            return response;
        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// Role DAL - Get Role By Id

        /// Description:- Retrieves the details of a specific role from the database using the role Id.

        /// </summary>

        public async Task<APIGetResponseModel<RoleModel>> GetById(long id, string email, IDbTransaction? transaction = null)

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

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);
                param.Add("p_OrganizationId", null);

                param.Add("p_UserEmail", email);

                var data =await conn.QueryFirstOrDefaultAsync<RoleModel>("sp_manage_role", param, commandType: CommandType.StoredProcedure);

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
                Console.WriteLine("DAL ROLE GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// Role DAL - Create Role

        /// Description:- Inserts a new role record into the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(RoleRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

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

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_role", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }
            catch (Exception ex)

            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting role");

                Console.WriteLine("DAL ROLE INSERT ERROR: " + ex.Message);
            }

            return response;

        }

        // ========================

        // UPDATE

        // ========================

        /// <summary>

        /// Role DAL - Update Role

        /// Description:- Updates the existing role information in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Update(RoleRequestDto request, string email, IDbTransaction? transaction = null)

        {
            var response = new APIGetResponseModel<int>();

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

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_OrganizationId", request.OrganizationId, DbType.Int64);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_role", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }
            return response;
        }

        // ========================

        // CHANGE STATUS

        // ========================

        /// <summary>

        /// Role DAL - Change Role Status

        /// Description:- Updates the active or inactive status of the specified role in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_RoleId", id);

                param.Add("p_RoleName", null);

                param.Add("p_RoleCode", null);

                param.Add("p_Description", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_OrganizationId", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_role", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while changing role status");

                Console.WriteLine("DAL ROLE STATUS ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // DROPDOWN

        // ========================

        /// <summary>

        /// Role DAL - Get Role Dropdown

        /// Description:- Retrieves the role dropdown list from the database for UI selection controls.

        /// </summary>

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<DropdownModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");

                param.Add("p_RoleId", null);

                param.Add("p_RoleName", null);

                param.Add("p_RoleCode", null);

                param.Add("p_Description", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);
                param.Add("p_OrganizationId", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_role", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = data.Any();

            }
            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching role dropdown");

                Console.WriteLine("DAL ROLE DROPDOWN ERROR: " + ex.Message);
            }

            return response;

        }
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdownByOrganization(
    long organizationId,
    string email,
    IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            using var conn = new MySqlConnection(_config.DefaultConnection);

            var param = new DynamicParameters();

            param.Add("p_Action", "DROPDOWN_BY_ORGANIZATION");

            param.Add("p_RoleId", 0);
            param.Add("p_RoleName", "");
            param.Add("p_RoleCode", "");
            param.Add("p_Description", "");
            param.Add("p_Status", true);
            param.Add("p_SearchKey", "");
            param.Add("p_PageNo", 1);

            param.Add("p_OrganizationId", organizationId);
            param.Add("p_UserEmail", email);

            var data = await conn.QueryAsync<DropdownModel>(
                "sp_manage_role",
                param,
                commandType: CommandType.StoredProcedure);

            response.IsSuccess = true;
            response.Result = data.ToList();

            return response;
        }
    }

}
