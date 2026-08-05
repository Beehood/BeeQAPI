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
    public class DAL_Branch : IDAL_Branch

    {
        private readonly DBConnection _config;
        public DAL_Branch(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================

        /// <summary>

        /// Branch DAL - Get All Branches

        /// Description:- Retrieves all branch records from the database with pagination and search functionality.

        /// </summary>

        public async Task<APIGetResponseModel<List<BranchModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<BranchModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_BranchId", null);

                param.Add("p_BranchName", null);

                param.Add("p_Address", null);

                param.Add("p_City", null);

                param.Add("p_State", null);

                param.Add("p_Country", null);

                param.Add("p_Timezone", null);

                param.Add("p_OrganizationId", null);

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_branch", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<BranchModel>()).ToList();

                response.Result = list;

                response.IsSuccess = true;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching branches");

                Console.WriteLine("DAL BRANCH GET ALL ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// Branch DAL - Get Branch By Id

        /// Description:- Retrieves the details of a specific branch from the database using the branch Id.

        /// </summary>

        public async Task<APIGetResponseModel<BranchModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<BranchModel>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_BranchId", id);

                param.Add("p_BranchName", null);

                param.Add("p_Address", null);

                param.Add("p_City", null);

                param.Add("p_State", null);

                param.Add("p_Country", null);

                param.Add("p_Timezone", null);

                param.Add("p_OrganizationId", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<BranchModel>("sp_manage_branch", param, commandType: CommandType.StoredProcedure);

                if (data != null)

                {

                    response.Result = data;

                    response.TotalRecords = 1;

                    response.IsSuccess = true;

                }
                else{
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching branch");

                Console.WriteLine("DAL BRANCH GETBYID ERROR: " + ex.Message);
            }

            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// Branch DAL - Create Branch

        /// Description:- Inserts a new branch record into the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(BranchRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_BranchId", null);

                param.Add("p_BranchName", request.BranchName);

                param.Add("p_Address", request.Address);

                param.Add("p_City", request.City);

                param.Add("p_State", request.State);

                param.Add("p_Country", request.Country);

                param.Add("p_Timezone", request.Timezone);

                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_branch", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting branch");

                Console.WriteLine("DAL BRANCH INSERT ERROR: " + ex.Message);
            }

            return response;

        }

        // ========================

        // UPDATE

        // ========================

        /// <summary>

        /// Branch DAL - Update Branch

        /// Description:- Updates the existing branch information in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Update(BranchRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_BranchName", request.BranchName);

                param.Add("p_Address", request.Address);

                param.Add("p_City", request.City);

                param.Add("p_State", request.State);

                param.Add("p_Country", request.Country);

                param.Add("p_Timezone", request.Timezone);

                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_branch", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while updating branch");

                Console.WriteLine("DAL BRANCH UPDATE ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // CHANGE STATUS

        // ========================

        /// <summary>

        /// Branch DAL - Change Branch Status

        /// Description:- Updates the active or inactive status of the specified branch in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_BranchId", id);

                param.Add("p_BranchName", null);

                param.Add("p_Address", null);

                param.Add("p_City", null);

                param.Add("p_State", null);

                param.Add("p_Country", null);

                param.Add("p_Timezone", null);

                param.Add("p_OrganizationId", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_branch", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while changing branch status");

                Console.WriteLine("DAL BRANCH STATUS ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // DROPDOWN

        // ========================

        /// <summary>

        /// Branch DAL - Get Branch Dropdown

        /// Description:- Retrieves the branch dropdown list from the database for UI selection controls.

        /// </summary>

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,long? organizationId,IDbTransaction? transaction = null)

        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");

                param.Add("p_BranchId", null);

                param.Add("p_BranchName", null);

                param.Add("p_Address", null);

                param.Add("p_City", null);

                param.Add("p_State", null);

                param.Add("p_Country", null);

                param.Add("p_Timezone", null);

                param.Add("p_OrganizationId", organizationId);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_branch", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = data.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching branch dropdown");

                Console.WriteLine("DAL BRANCH DROPDOWN ERROR: " + ex.Message);
            }
            return response;
        }
    }
}
