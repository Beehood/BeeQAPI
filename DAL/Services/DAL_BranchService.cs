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
        // GET ALL (Dynamic - Multi Result)
        // ========================
        /// <summary>
        /// Branch Service DAL - Get All Branch Services
        /// Author: Swapnlisa
        /// Description:- Fetches paginated branch service list using stored procedure (multi-result).
        /// </summary>
        public async Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null)
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
                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                //param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserId", null);

                using var multi = await conn.QueryMultipleAsync(
                    "sp_manage_branch_service",
                    param,
                    commandType: CommandType.StoredProcedure);

                // 1st Result → Total Count
                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // 2nd Result → Data
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
        /// Author: Swapnlisa
        /// Description:- Fetches single branch service using BranchServiceId.
        /// </summary>
        public async Task<APIGetResponseModel<BranchServiceModel>> GetById(long id, IDbTransaction? transaction = null)
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
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", null);

                var data = await conn.QueryFirstOrDefaultAsync<BranchServiceModel>(
                    "sp_manage_branch_service",
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
                response.ErrorMsgs.Add("Error while fetching branch service");
                Console.WriteLine("DAL GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        /// <summary>
        /// Branch Service DAL - Insert Branch Service
        /// Author: Swapnlisa
        /// Description:- Inserts new branch service record using stored procedure.
        /// </summary>
        public async Task<APIGetResponseModel<long>> Insert(BranchServiceRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

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
                param.Add("p_Status", 1);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure);
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
        /// Branch Service DAL - Update Branch Service
        /// Author: Swapnlisa
        /// Description:- Updates existing branch service details.
        /// </summary>
        public async Task<APIGetResponseModel<long>> Update(BranchServiceRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

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
                param.Add("p_Status", request.Status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);
                var id = await conn.ExecuteScalarAsync<long>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure);
                response.Result = id;
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
        /// Branch Service DAL - Change Status
        /// Author: Swapnlisa
        /// Description:- Updates branch service status (Active/Inactive).
        /// </summary>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

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
                param.Add("p_Status", status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var result = await conn.ExecuteScalarAsync<long>("sp_manage_branch_service", param, commandType: CommandType.StoredProcedure);

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
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var data = (await conn.QueryAsync<DropdownModel>(@"SELECTbranch_id AS Id,branch_name AS NameFROM branchesWHERE status = 1"))
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