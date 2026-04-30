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
        // GET ALL (Dynamic - Multi Result)
        /// <summary>
        /// Branch DAL - Get All Branches
        /// Author: Swapnlisa
        /// Description:- Fetches paginated branch list using stored procedure (multi-result).
        /// <param name="request">PaginationRequestDto</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns list of BranchModel with total count</returns>
        // ========================
        public async Task<APIGetResponseModel<List<BranchModel>>> GetAll(PaginationRequestDto request,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<BranchModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                // Required because stored procedure expects all parameters
                param.Add("p_BranchId", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchName", null);
                param.Add("p_Address", null);
                param.Add("p_City", null);
                param.Add("p_State", null);
                param.Add("p_Country", null);
                param.Add("p_Timezone", null);
                param.Add("p_Status", null);

                // Actual pagination values
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserId", null);
                using var multi = await conn.QueryMultipleAsync("sp_manage_branch",param,commandType: CommandType.StoredProcedure);

                // 🔹 1st Result → Total Count
                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // 🔹 2nd Result → Data
                var list = (await multi.ReadAsync<BranchModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
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
        /// Author: Swapnlisa
        /// Description:- Fetches single branch using BranchId.
        /// <param name="id">BranchId</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns BranchModel</returns>
        public async Task<APIGetResponseModel<BranchModel>> GetById(long id,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<BranchModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_BranchId", id);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchName", null);
                param.Add("p_Address", null);
                param.Add("p_City", null);
                param.Add("p_State", null);
                param.Add("p_Country", null);
                param.Add("p_Timezone", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", null);
                var data = await conn.QueryFirstOrDefaultAsync<BranchModel>("sp_manage_branch",param,commandType: CommandType.StoredProcedure);

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
                response.ErrorMsgs.Add("Error while fetching branch");
                Console.WriteLine("DAL BRANCH GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        /// <summary>
        /// Branch DAL - Insert Branch
        /// Author: Swapnlisa
        /// Description:- Inserts new branch record using stored procedure.
        /// <param name="request">BranchRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns newly created BranchId</returns>
        public async Task<APIGetResponseModel<long>> Insert(BranchRequestDto request,string userId,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_BranchId", null);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchName", request.BranchName);
                param.Add("p_Address", request.Address);
                param.Add("p_City", request.City);
                param.Add("p_State", request.State);
                param.Add("p_Country", request.Country);
                param.Add("p_Timezone", request.Timezone);
                param.Add("p_Status", 1);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_branch",param,commandType: CommandType.StoredProcedure);

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while inserting branch");
                Console.WriteLine("DAL INSERT ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// Branch DAL - Update Branch
        /// Author: Swapnlisa
        /// Description:- Updates existing branch details.
        /// <param name="request">BranchRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns updated BranchId</returns>
        public async Task<APIGetResponseModel<long>> Update(BranchRequestDto request,string userId,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_BranchId", request.BranchId);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchName", request.BranchName);
                param.Add("p_Address", request.Address);
                param.Add("p_City", request.City);
                param.Add("p_State", request.State);
                param.Add("p_Country", request.Country);
                param.Add("p_Timezone", request.Timezone);
                param.Add("p_Status", request.Status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_branch",param,commandType: CommandType.StoredProcedure);
                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating branch");
                Console.WriteLine("DAL UPDATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Branch DAL - Change Status
        /// Author: Swapnlisa
        /// Description:- Updates branch status (Active/Inactive).
        /// <param name="id">BranchId</param>
        /// <param name="status">0 = Inactive, 1 = Active</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns status update result</returns>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id,int status,long userId,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_BranchId", id);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchName", null);
                param.Add("p_Address", null);
                param.Add("p_City", null);
                param.Add("p_State", null);
                param.Add("p_Country", null);
                param.Add("p_Timezone", null);
                param.Add("p_Status", status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var result = await conn.ExecuteScalarAsync<long>("sp_manage_branch",param,commandType: CommandType.StoredProcedure );

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
    }
}
