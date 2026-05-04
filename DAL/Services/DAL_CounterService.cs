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
        // GET ALL (Dynamic - Multi Result)
        // ========================
        /// <summary>
        /// Counter Service DAL - Get All Counter Services
        /// Author: Swapnlisa
        /// Description:- Fetches paginated counter service list using stored procedure (multi-result).
        /// </summary>
        public async Task<APIGetResponseModel<List<CounterServiceModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null)
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
                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                //param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserId", null);

                using var multi = await conn.QueryMultipleAsync("sp_manage_counter_service", param, commandType: CommandType.StoredProcedure);

                // 1st Result → Total Count
                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // 2nd Result → Data
                var list = (await multi.ReadAsync<CounterServiceModel>()).ToList();

                response.Result = list;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);   // 👈 show real error
                Console.WriteLine("DAL GET ALL ERROR: " + ex.ToString());
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Counter Service DAL - Get Counter Service By Id
        /// Author: Swapnlisa
        /// Description:- Fetches single counter service using CounterServiceId.
        /// </summary>
        public async Task<APIGetResponseModel<CounterServiceModel>> GetById(long id, IDbTransaction? transaction = null)
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
                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", null);

                var data = await conn.QueryFirstOrDefaultAsync<CounterServiceModel>("sp_manage_counter_service", param, commandType: CommandType.StoredProcedure);

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
                response.ErrorMsgs.Add("Error while fetching counter service");
                Console.WriteLine("DAL GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        /// <summary>
        /// Counter Service DAL - Insert Counter Service
        /// Author: Swapnlisa
        /// Description:- Inserts new counter service record using stored procedure.
        /// </summary>
        public async Task<APIGetResponseModel<long>> Insert(CounterServiceRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_CounterServiceId", null);
                param.Add("p_CounterId", request.CounterId);
                param.Add("p_ServiceId", request.ServiceId);
                param.Add("p_Status", 1);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_counter_service", param, commandType: CommandType.StoredProcedure);

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
        /// Counter Service DAL - Update Counter Service
        /// Author: Swapnlisa
        /// Description:- Updates existing counter service details.
        /// </summary>
        public async Task<APIGetResponseModel<long>> Update(CounterServiceRequestDto request, string userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_CounterServiceId", request.CounterServiceId);
                param.Add("p_CounterId", request.CounterId);
                param.Add("p_ServiceId", request.ServiceId);
                param.Add("p_Status", request.Status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_counter_service", param, commandType: CommandType.StoredProcedure);

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);   // 👈 CRITICAL
                Console.WriteLine("DAL UPDATE ERROR: " + ex.ToString());
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Counter Service DAL - Change Status
        /// Author: Swapnlisa
        /// Description:- Updates counter service status (Active/Inactive).
        /// </summary>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_CounterServiceId", id);
                param.Add("p_CounterId", null);
                param.Add("p_ServiceId", null);
                param.Add("p_Status", status);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_PageSize", null);

                param.Add("p_UserId", userId);

                var result = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_counter_service",
                    param,
                    commandType: CommandType.StoredProcedure);

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
        /// Counter Service DAL - Get Counter Dropdown
        /// Author: Swapnlisa
        /// Description:- Fetches active counter dropdown list.
        /// </summary>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var data = (await conn.QueryAsync<DropdownModel>(@"SELECTcounter_id AS Id,counter_name AS NameFROM counters  WHERE status = 1"))
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
