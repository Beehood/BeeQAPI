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
    public class DAL_Counter : IDAL_Counter
    {
        private readonly DBConnection _config;

        public DAL_Counter(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL (Dynamic - Multi Result)
        // ========================
        /// <summary>
        /// Counter DAL - Get All Counters
        /// Author: Swapnlisa
        /// Description:- Fetches paginated counter list using stored procedure (multi-result).
        /// </summary>
        /// <param name="request">PaginationRequestDto</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns list of CounterModel with total count</returns>
        public async Task<APIGetResponseModel<List<CounterModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<CounterModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_CounterId", null);
                param.Add("p_BranchId", null);
                param.Add("p_CounterName", null);
                param.Add("p_CounterCode", null);
                param.Add("p_Description", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserId", null);

                using var multi = await conn.QueryMultipleAsync("sp_manage_counter",param,commandType: CommandType.StoredProcedure);

                // 🔹 1st Result → Total Count
                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // 🔹 2nd Result → Data
                var list = (await multi.ReadAsync<CounterModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
                Console.WriteLine("DAL COUNTER GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Counter DAL - Get Counter By Id
        /// Author: Swapnlisa
        /// Description:- Fetches single counter using CounterId.
        /// </summary>
        /// <param name="id">CounterId</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns CounterModel</returns>
        public async Task<APIGetResponseModel<CounterModel>> GetById(long id, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<CounterModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_CounterId", id);
                param.Add("p_BranchId", null);
                param.Add("p_CounterName", null);
                param.Add("p_CounterCode", null);
                param.Add("p_Description", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", null);

                var data = await conn.QueryFirstOrDefaultAsync<CounterModel>("sp_manage_counter",param,commandType: CommandType.StoredProcedure);

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
                response.ErrorMsgs.Add("Error while fetching counter");
                Console.WriteLine("DAL COUNTER GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        /// <summary>
        /// Counter DAL - Insert Counter
        /// Author: Swapnlisa
        /// Description:- Inserts new counter record using stored procedure.
        /// </summary>
        /// <param name="request">CounterRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns newly created CounterId</returns>
        public async Task<APIGetResponseModel<long>> Insert(CounterRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_CounterId", null);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_CounterName", request.CounterName);
                param.Add("p_CounterCode", request.CounterCode);
                param.Add("p_Status", 1);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_counter",param,commandType: CommandType.StoredProcedure);

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while inserting counter");
                Console.WriteLine("DAL COUNTER INSERT ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// Counter DAL - Update Counter
        /// Author: Swapnlisa
        /// Description:- Updates existing counter details.
        /// </summary>
        /// <param name="request">CounterRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns updated CounterId</returns>
        public async Task<APIGetResponseModel<long>> Update(CounterRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_CounterId", request.CounterId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_CounterName", request.CounterName);
                param.Add("p_CounterCode", request.CounterCode);
                param.Add("p_Status", request.Status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_counter",param,commandType: CommandType.StoredProcedure);

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating counter");
                Console.WriteLine("DAL COUNTER UPDATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Counter DAL - Change Status
        /// Author: Swapnlisa
        /// Description:- Updates counter status (Active/Inactive).
        /// </summary>
        /// <param name="id">CounterId</param>
        /// <param name="status">0 = Inactive, 1 = Active</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns status update result</returns>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_CounterId", id);
                param.Add("p_BranchId", null);
                param.Add("p_CounterName", null);
                param.Add("p_CounterCode", null);
                param.Add("p_Description", null);
                param.Add("p_Status", status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var result = await conn.ExecuteScalarAsync<long>("sp_manage_counter",param,commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = result > 0;
                response.TotalRecords = result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while changing counter status");
                Console.WriteLine("DAL COUNTER STATUS ERROR: " + ex.Message);
            }

            return response;
        }
    }
}
