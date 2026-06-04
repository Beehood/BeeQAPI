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
    public class DAL_Queue : IDAL_Queue
    {
        private readonly DBConnection _config;

        public DAL_Queue(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL (QUEUE LIST)
        // ========================
        public async Task<APIGetResponseModel<List<QueueModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<QueueModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "LIST");
                param.Add("p_TokenId", null);
                param.Add("p_BranchId", null);
                param.Add("p_BranchServiceId", null);
                param.Add("p_TokenNumber", null);
                param.Add("p_CustomerName", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_queue", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<QueueModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching queue");
                Console.WriteLine("DAL QUEUE GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<QueueModel>> GetById(long tokenId, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<QueueModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "GETBYID");
                param.Add("p_TokenId", tokenId);
                param.Add("p_BranchId", null);
                param.Add("p_BranchServiceId", null);
                param.Add("p_TokenNumber", null);
                param.Add("p_CustomerName", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<QueueModel>("sp_manage_queue", param, commandType: CommandType.StoredProcedure);

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
                response.ErrorMsgs.Add("Error while fetching queue");
                Console.WriteLine("DAL QUEUE GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT (CREATE TOKEN)
        // ========================
        public async Task<APIGetResponseModel<int>> Insert( QueueRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            using var conn = new MySqlConnection(_config.DefaultConnection);
            await conn.OpenAsync();

            using var tran = conn.BeginTransaction();

            try
            {
                var param = new DynamicParameters();
                param.Add("p_Action", "INSERT");
                param.Add("p_TokenId", null);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_BranchServiceId", request.BranchServiceId);
                param.Add("p_TokenNumber", null);
                param.Add("p_CustomerName", request.CustomerName);
                param.Add("p_Status", 1);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_queue",param,transaction: tran,commandType: CommandType.StoredProcedure);

                tran.Commit();

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                tran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while creating token");

                Console.WriteLine("DAL QUEUE INSERT ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<int>> Update(QueueRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            using var conn = new MySqlConnection(_config.DefaultConnection);
            await conn.OpenAsync();

            using var tran = conn.BeginTransaction();

            try
            {
                var param = new DynamicParameters();
                param.Add("p_Action", "UPDATE");
                param.Add("p_TokenId", request.TokenId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_BranchServiceId", request.BranchServiceId);
                param.Add("p_TokenNumber", null);
                param.Add("p_CustomerName", request.CustomerName);
                param.Add("p_Status", request.Priority);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_queue",param,transaction: tran,commandType: CommandType.StoredProcedure);

                tran.Commit();

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating queue");

                Console.WriteLine("DAL QUEUE UPDATE ERROR: " + ex.Message);
            }

            return response;
        }
        // ========================
        // CHANGE STATUS (CALL / COMPLETE)
        // ========================
        public async Task<APIGetResponseModel<int>> ChangeStatus(QueueRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            using var conn = new MySqlConnection(_config.DefaultConnection);
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction();

            try
            {
             

                var param = new DynamicParameters();
                param.Add("p_Action", request.Action);
                param.Add("p_TokenId", request.TokenId);
                param.Add("p_BranchId", null);
                param.Add("p_BranchServiceId", null);
                param.Add("p_TokenNumber", null);
                param.Add("p_CustomerName", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_queue", param, transaction: tran, commandType: CommandType.StoredProcedure);
                tran.Commit();

                response.Result = result;
                response.IsSuccess = result > 0;
                response.TotalRecords = result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                tran.Rollback(); // ❌ FAIL SAFE

                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating queue status");

                Console.WriteLine("DAL QUEUE STATUS ERROR: " + ex.Message);
            }

            return response;

        
        }

        // ========================
        // QUEUE DISPLAY (MONITOR)
        // ========================
        public async Task<APIGetResponseModel<List<QueueDisplayModel>>> GetQueueDisplay(string branchId)
        {
            var response = new APIGetResponseModel<List<QueueDisplayModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "CURRENT");
                param.Add("p_TokenId", 0);
                param.Add("p_CounterId", 0);

                param.Add("p_BranchId", Convert.ToInt64(branchId));

                param.Add("p_BranchServiceId", null);
                param.Add("p_TokenNumber", null);
                param.Add("p_CustomerName", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", "");

                var data = (await conn.QueryAsync<QueueDisplayModel>("sp_manage_queue",param,commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = data.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching queue display");
                Console.WriteLine("DAL QUEUE DISPLAY ERROR: " + ex.Message);
            }

            return response;
        }
        // ========================
        // DROPDOWN
        // ========================
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "DROPDOWN");
                param.Add("p_TokenId", null);
                param.Add("p_BranchId", null);
                param.Add("p_BranchServiceId", null);
                param.Add("p_TokenNumber", null);
                param.Add("p_CustomerName", null);
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_queue",param,commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = data.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching dropdown");
                Console.WriteLine("DAL QUEUE DROPDOWN ERROR: " + ex.Message);
            }

            return response;
        }
    }
}

