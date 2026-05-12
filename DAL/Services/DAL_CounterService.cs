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
    public class DAL_CounterService : IDAL_CounterService
    {
        private readonly DBConnection _config;

        public DAL_CounterService(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<CounterServiceModel>>> GetAll(PaginationRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<CounterServiceModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "LIST");
                param.Add("p_CounterServiceId", null);
                param.Add("p_CounterId", null);
                param.Add("p_ServiceId", null);
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync(
                    "sp_manage_counter_service",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                // Count
                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // Data
                var list = (await multi.ReadAsync<CounterServiceModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching counter services");
                Console.WriteLine("DAL COUNTER SERVICE GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<CounterServiceModel>> GetById(
            long id,
            string email,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<CounterServiceModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "GETBYID");
                param.Add("p_CounterServiceId", id);
                param.Add("p_CounterId", null);
                param.Add("p_ServiceId", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<CounterServiceModel>("sp_manage_counter_service",param,commandType: CommandType.StoredProcedure);

                if (data != null)
                {
                    response.Result = data;
                    response.TotalRecords = 1;
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching counter service");
                Console.WriteLine("DAL COUNTER SERVICE GETBYID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        public async Task<APIGetResponseModel<int>> Insert(CounterServiceRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "INSERT");
                param.Add("p_CounterServiceId", null);
                param.Add("p_CounterId", request.CounterId);
                param.Add("p_ServiceId", request.ServiceId);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_counter_service",param,commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while inserting counter service");
                Console.WriteLine("DAL COUNTER SERVICE INSERT ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<int>> Update(CounterServiceRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "UPDATE");
                param.Add("p_CounterServiceId", request.CounterServiceId);
                param.Add("p_CounterId", request.CounterId);
                param.Add("p_ServiceId", request.ServiceId);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_counter_service",param,commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating counter service");
                Console.WriteLine("DAL COUNTER SERVICE UPDATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // STATUS
        // ========================
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "STATUS");
                param.Add("p_CounterServiceId", id);
                param.Add("p_CounterId", null);
                param.Add("p_ServiceId", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_counter_service",param,commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = result > 0;
                response.TotalRecords = result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while changing status");
                Console.WriteLine("DAL COUNTER SERVICE STATUS ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "DROPDOWN");
                param.Add("p_CounterServiceId", null);
                param.Add("p_CounterId", null);
                param.Add("p_ServiceId", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_counter_service",param,commandType: CommandType.StoredProcedure  )).ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = data.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching dropdown");
                Console.WriteLine("DAL COUNTER SERVICE DROPDOWN ERROR: " + ex.Message);
            }

            return response;
        }
    }
}
