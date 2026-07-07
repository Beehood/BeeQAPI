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

        // GET ALL

        // ========================

        /// <summary>

        /// User DAL - Get All Users

        /// Description:- Retrieves all user records from the database with pagination and search functionality.

        /// </summary>

        public async Task<APIGetResponseModel<List<UserModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

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

                param.Add("p_RoleId", null);

                param.Add("p_Name", null);

                param.Add("p_Email", null);

                param.Add("p_Phone", null);

                param.Add("p_Password", null);

                param.Add("p_Status", null);


                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_user", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<UserModel>()).ToList();

                response.Result = list;

                response.IsSuccess = list.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching users");

                Console.WriteLine("DAL USER GET ALL ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// User DAL - Get User By Id

        /// Description:- Retrieves the details of a specific user from the database using the user Id.

        /// </summary>

        public async Task<APIGetResponseModel<UserModel>> GetById(long id, string email, IDbTransaction? transaction = null)

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

                param.Add("p_RoleId", null);

                param.Add("p_Name", null);

                param.Add("p_Email", null);

                param.Add("p_Phone", null);

                param.Add("p_Password", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<UserModel>("sp_manage_user", param, commandType: CommandType.StoredProcedure);

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

                response.ErrorMsgs.Add("Error while fetching user");

                Console.WriteLine("DAL USER GETBYID ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// User DAL - Create User

        /// Description:- Inserts a new user record into the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(UserRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_UserId", null);

                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_RoleId", request.RoleId);

                param.Add("p_Name", request.Name);

                param.Add("p_Email", request.Email);

                //param.Add("p_Phone", request.Phone);

                param.Add("p_Password", request.Password);

                param.Add("p_Status", 1);


                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_user", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting user");

                Console.WriteLine("DAL USER INSERT ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // UPDATE

        // ========================

        /// <summary>

        /// User DAL - Update User

        /// Description:- Updates the existing user information in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Update(UserRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_UserId", request.UserId);

                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_RoleId", request.RoleId);

                param.Add("p_Name", request.Name);

                param.Add("p_Email", request.Email);

                //param.Add("p_Phone",request.Phone);

                param.Add("p_Password", request.Password);

                param.Add("p_Status", request.Status);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_user", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while updating user");

                Console.WriteLine("DAL USER UPDATE ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // CHANGE STATUS

        // ========================

        /// <summary>

        /// User DAL - Change User Status

        /// Description:- Updates the active or inactive status of the specified user in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_UserId", id);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_RoleId", null);

                param.Add("p_Name", null);

                param.Add("p_Email", null);

                param.Add("p_Phone", null);

                param.Add("p_Password", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_user", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while changing user status");

                Console.WriteLine("DAL USER STATUS ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // DROPDOWN

        // ========================

        /// <summary>

        /// User DAL - Get User Dropdown

        /// Description:- Retrieves the user dropdown list from the database for UI selection controls.

        /// </summary>

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<DropdownModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");

                param.Add("p_UserId", null);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_RoleId", null);

                param.Add("p_Name", null);

                param.Add("p_Email", null);

                param.Add("p_Phone", null);

                param.Add("p_Password", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_user", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = data.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching user dropdown");

                Console.WriteLine("DAL USER DROPDOWN ERROR: " + ex.Message);

            }

            return response;

        }

    }

}
