using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Microsoft.Extensions.Logging;
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
    public class DAL_CounterPanel : IDAL_CounterPanel
    {
        private readonly DBConnection _config;
        private readonly ILogger<DAL_CounterPanel> _logger;
        public DAL_CounterPanel(DBConnection config, ILogger<DAL_CounterPanel> logger)
        {
            _config = config;
            _logger = logger;
        }

        // ========================
        // DASHBOARD
        // ========================

        /// <summary>

        /// Counter Panel DAL - Get Dashboard

        /// Description:- Retrieves the counter dashboard information, including the current serving token, waiting queue, missed queue, and counter statistics.

        /// </summary>

        public async Task<APIGetResponseModel<CounterPanelDashboardModel>> GetDashboard(CounterPanelActionRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<CounterPanelDashboardModel>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                await conn.OpenAsync();

                var param = new DynamicParameters();

                param.Add("p_Action", "DASHBOARD");

                param.Add("p_counter_id", request.CounterId);

                param.Add("p_token_id", null);

                param.Add("p_branch_service_id", request.BranchServiceId);

                param.Add("p_user_id", null);

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_PageSize", request.PageSize);

                param.Add("p_Email", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_counter_panel", param, commandType: CommandType.StoredProcedure, commandTimeout: 30);

                var currentServing = await multi.ReadFirstOrDefaultAsync<CurrentServingModel>();

                var waitingQueue = (await multi.ReadAsync<WaitingQueueModel>()).ToList();

                var missedQueue = (await multi.ReadAsync<MissedQueueModel>()).ToList();

                var stats = await multi.ReadFirstOrDefaultAsync<CounterPanelStatsModel>();

                response.Result = new CounterPanelDashboardModel

                {
                    CurrentServing = currentServing,
                    WaitingQueue = waitingQueue,
                    MissedQueue = missedQueue,
                    Stats = stats ?? new CounterPanelStatsModel()
                };

                response.IsSuccess = true;

                response.TotalRecords = waitingQueue.Count;

            }
            catch (Exception ex)

            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);

                _logger.LogError(ex, "DAL COUNTER DASHBOARD ERROR");
            }

            return response;

        }

        // ========================

        // CALL NEXT TOKEN

        // ========================

        /// <summary>

        /// Counter Panel DAL - Call Next Token

        /// Description:- Retrieves and assigns the next available queue token to the selected counter for service.

        /// </summary>

        public async Task<APIGetResponseModel<CallNextTokenResponseDto>> CallNextToken(CounterPanelActionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<CallNextTokenResponseDto>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                await conn.OpenAsync();

                _logger.LogInformation("JWT VALUE = {JwtValue}", email);

                var userId = await conn.QueryFirstOrDefaultAsync<long>("SELECT user_id\r\nFROM users\r\nWHERE email = @Email\r\nLIMIT 1;", new { Email = email });

                _logger.LogInformation("JWT VALUE = {Value}", email);

                _logger.LogInformation("USER ID = {UserId}", userId);

                var param = new DynamicParameters();

                param.Add("p_counter", request.CounterId);

                param.Add("p_user", userId);

                param.Add("p_branch_service_id", request.BranchServiceId);

                var data = await conn.QueryFirstOrDefaultAsync<CallNextTokenResponseDto>("sp_call_next_token", param, commandType: CommandType.StoredProcedure, commandTimeout: 30);

                response.Result = data;

                response.IsSuccess = data != null;

                response.TotalRecords = data != null ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);

                _logger.LogError(ex, "DAL COUNTER CALLNEXT ERROR");

            }

            return response;

        }

        // ========================

        // START SERVICE

        // ========================

        /// <summary>

        /// Counter Panel DAL - Start Service

        /// Description:- Updates the selected queue token status to indicate that the service has started.

        /// </summary>

        public async Task<APIGetResponseModel<int>> StartService(CounterPanelActionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                await conn.OpenAsync();

                var param = new DynamicParameters();

                param.Add("p_Action", "STARTSERVICE");

                param.Add("p_counter_id", request.CounterId);

                param.Add("p_token_id", request.TokenId);

                param.Add("p_branch_service_id", request.BranchServiceId);

                param.Add("p_user_id", null);

                param.Add("p_SearchKey", "");

                param.Add("p_PageNo", 1);

                param.Add("p_PageSize", 10);

                param.Add("p_Email", email);

                var result = await conn.ExecuteScalarAsync<int>

                    (

                        "sp_manage_counter_panel", param, commandType: CommandType.StoredProcedure, commandTimeout: 30

                    );

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while starting service");

                _logger.LogError(ex, "DAL START SERVICE ERROR");

            }

            return response;

        }

        // ========================

        // COMPLETE SERVICE

        // ========================

        /// <summary>

        /// Counter Panel DAL - Complete Service

        /// Description:- Updates the selected queue token status to indicate that the service has been completed.

        /// </summary>

        public async Task<APIGetResponseModel<int>> CompleteService(CounterPanelActionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                await conn.OpenAsync();

                var param = new DynamicParameters();

                param.Add("p_Action", "COMPLETESERVICE");

                param.Add("p_counter_id", request.CounterId);

                param.Add("p_token_id", request.TokenId);

                param.Add("p_branch_service_id", request.BranchServiceId);

                param.Add("p_user_id", null);

                param.Add("p_SearchKey", "");

                param.Add("p_PageNo", 1);

                param.Add("p_PageSize", 10);

                param.Add("p_Email", email);

                var result = await conn.ExecuteScalarAsync<int>

                    (

                        "sp_manage_counter_panel", param, commandType: CommandType.StoredProcedure, commandTimeout: 30

                    );

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while completing service");

                _logger.LogError(ex, "DAL COMPLETE SERVICE ERROR");

            }

            return response;

        }

        // ========================

        // SKIP TOKEN

        // ========================

        /// <summary>

        /// Counter Panel DAL - Skip Token

        /// Description:- Updates the selected queue token status by marking it as skipped.

        /// </summary>

        public async Task<APIGetResponseModel<int>> SkipToken(CounterPanelActionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                await conn.OpenAsync();

                var param = new DynamicParameters();

                param.Add("p_Action", "SKIPTOKEN");

                param.Add("p_counter_id", request.CounterId);

                param.Add("p_token_id", request.TokenId);

                param.Add("p_branch_service_id", request.BranchServiceId);

                param.Add("p_user_id", null);

                param.Add("p_SearchKey", "");

                param.Add("p_PageNo", 1);

                param.Add("p_PageSize", 10);

                param.Add("p_Email", email);

                var result = await conn.ExecuteScalarAsync<int>

                    (

                        "sp_manage_counter_panel", param, commandType: CommandType.StoredProcedure, commandTimeout: 30

                    );

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while skipping token");

                _logger.LogError(ex, "DAL SKIP TOKEN ERROR");

            }

            return response;

        }

        // ========================

        // RECALL TOKEN

        // ========================

        /// <summary>

        /// Counter Panel DAL - Recall Token

        /// Description:- Recalls the previously called queue token for the selected counter.

        /// </summary>

        public async Task<APIGetResponseModel<int>> RecallToken(CounterPanelActionRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                await conn.OpenAsync();

                var param = new DynamicParameters();

                param.Add("p_Action", "RECALLTOKEN");

                param.Add("p_counter_id", request.CounterId);

                param.Add("p_token_id", request.TokenId);

                param.Add("p_branch_service_id", request.BranchServiceId);

                param.Add("p_user_id", null);

                param.Add("p_SearchKey", "");

                param.Add("p_PageNo", 1);

                param.Add("p_PageSize", 10);

                param.Add("p_Email", email);

                var result = await conn.ExecuteScalarAsync<int>

                    (

                        "sp_manage_counter_panel", param, commandType: CommandType.StoredProcedure, commandTimeout: 30

                    );

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (MySqlException ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);

                _logger.LogError(ex, "DAL RECALL TOKEN ERROR");

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Unexpected error");

                _logger.LogError(ex, "DAL RECALL TOKEN ERROR");

            }

            return response;

        }

    }

}

