using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace DAL.Implementation

{

    public class DAL_BranchService : IDAL_BranchService

    {
        private readonly DBConnection _config;
        public DAL_BranchService(DBConnection config)
        {
            _config = config;
        }

        // ========================

        // GET ALL

        // ========================

        /// <summary>

        /// Branch Service DAL - Get All Branch Services

        /// Description:- Retrieves all branch service records from the database with pagination and search functionality.

        /// </summary>

        public async Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<BranchServiceModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_BranchServiceId", null);

                param.Add("p_BranchId", null);

                param.Add("p_ServiceId", null);

                param.Add("p_Prefix", null);

                param.Add("p_DailyLimit", null);

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<BranchServiceModel>()).ToList();

                response.Result = list;

                response.IsSuccess = list.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching branch services");

                Console.WriteLine("DAL GET ALL ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// Branch Service DAL - Get Branch Service By Id

        /// Description:- Retrieves the details of a specific branch service from the database using the branch service Id.

        /// </summary>

        public async Task<APIGetResponseModel<BranchServiceModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<BranchServiceModel>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_BranchServiceId", id);

                param.Add("p_BranchId", null);

                param.Add("p_ServiceId", null);

                param.Add("p_Prefix", null);

                param.Add("p_DailyLimit", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<BranchServiceModel>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure);

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

                response.ErrorMsgs.Add("Error while fetching branch service");

                Console.WriteLine("DAL GET BY ID ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// Branch Service DAL - Create Branch Service

        /// Description:- Inserts a new branch service record into the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(BranchServiceRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_BranchServiceId", null);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_ServiceId", request.ServiceId);

                param.Add("p_Prefix", request.Prefix);

                param.Add("p_DailyLimit", request.DailyLimit);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting branch service");

                Console.WriteLine("DAL INSERT ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // UPDATE

        // ========================

        /// <summary>

        /// Branch Service DAL - Update Branch Service

        /// Description:- Updates the existing branch service information in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Update(BranchServiceRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_BranchServiceId", request.BranchServiceId);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_ServiceId", request.ServiceId);

                param.Add("p_Prefix", request.Prefix);

                param.Add("p_DailyLimit", request.DailyLimit);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while updating branch service");

                Console.WriteLine("DAL UPDATE ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // CHANGE STATUS

        // ========================

        /// <summary>

        /// Branch Service DAL - Change Branch Service Status

        /// Description:- Updates the active or inactive status of the specified branch service in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_BranchServiceId", id);

                param.Add("p_BranchId", null);

                param.Add("p_ServiceId", null);

                param.Add("p_Prefix", null);

                param.Add("p_DailyLimit", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure);

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

        /// Branch Service DAL - Get Branch Service Dropdown

        /// Description:- Retrieves the branch service dropdown list from the database for UI selection controls.

        /// </summary>

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<DropdownModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");

                param.Add("p_BranchServiceId", null);

                param.Add("p_BranchId", null);

                param.Add("p_ServiceId", null);

                param.Add("p_Prefix", null);

                param.Add("p_DailyLimit", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure)).ToList();

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

        // ========================

        // BRANCH DROPDOWN BY ORGANIZATION

        // ========================

        /// <summary>

        /// Branch Service DAL - Get Branch Dropdown By Organization

        /// Description:- Retrieves the list of branches associated with the specified organization for dropdown selection.

        /// </summary>

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetBranchDropdownByOrganization(long orgId, string email, IDbTransaction? transaction = null

        )

        {

            var response = new APIGetResponseModel<List<DropdownModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN_BY_ORG");

                param.Add("p_BranchServiceId", null);

                param.Add("p_BranchId", orgId);

                param.Add("p_ServiceId", null);

                param.Add("p_Prefix", null);

                param.Add("p_DailyLimit", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = true;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching branch dropdown");

                Console.WriteLine("DAL DROPDOWN_BY_ORG ERROR: " + ex.Message);

            }

            return response;

        }

    }

}
