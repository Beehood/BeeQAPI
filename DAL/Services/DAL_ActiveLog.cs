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

    public class DAL_ActiveLog : IDAL_ActiveLog

    {

        private readonly DBConnection _config;

        public DAL_ActiveLog(DBConnection config)

        {

            _config = config;

        }

        // ========================

        // GET ALL

        // ========================

        /// <summary>

        /// Activity Log DAL - Get All Activity Logs

        /// Description:- Fetch all activity logs.

        /// Returns activity history ordered by latest logs.

        /// </summary>

        public async Task<APIGetResponseModel<List<ActivityLogModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<ActivityLogModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_LogId", null);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_UserId", null);

                param.Add("p_UserName", null);

                param.Add("p_RoleName", null);

                param.Add("p_ActionName", null);

                param.Add("p_ModuleName", null);

                param.Add("p_TableName", null);

                param.Add("p_RecordId", null);

                param.Add("p_Description", null);

                param.Add("p_OldData", null);

                param.Add("p_NewData", null);

                param.Add("p_IpAddress", null);

                param.Add("p_DeviceInfo", null);

                using var multi = await conn.QueryMultipleAsync("sp_activity_logs", param, commandType: CommandType.StoredProcedure);

                var list = (await multi.ReadAsync<ActivityLogModel>()).ToList();

                response.Result = list;

                response.TotalRecords = list.Count;

                response.IsSuccess = true;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching activity logs");

                Console.WriteLine("DAL ACTIVITY LOG GET ALL ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// Activity Log DAL - Get Activity Log By Id

        /// Description:- Fetch single activity log details using LogId.

        /// </summary>

        public async Task<APIGetResponseModel<ActivityLogModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<ActivityLogModel>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_LogId", id);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_UserId", null);

                param.Add("p_UserName", null);

                param.Add("p_RoleName", null);

                param.Add("p_ActionName", null);

                param.Add("p_ModuleName", null);

                param.Add("p_TableName", null);

                param.Add("p_RecordId", null);

                param.Add("p_Description", null);

                param.Add("p_OldData", null);

                param.Add("p_NewData", null);

                param.Add("p_IpAddress", null);

                param.Add("p_DeviceInfo", null);

                var data = await conn.QueryFirstOrDefaultAsync<ActivityLogModel>("sp_activity_logs", param, commandType: CommandType.StoredProcedure);

                response.Result = data;

                response.IsSuccess = data != null;

                response.TotalRecords = data != null ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching activity log");

                Console.WriteLine("DAL ACTIVITY LOG GETBYID ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// Activity Log DAL - Insert Activity Log

        /// Description:- Inserts new activity log record.

        /// Used for audit tracking of user actions.

        /// Returns newly created LogId.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(ActivityLogRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_LogId", null);

                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_UserId", request.UserId);

                param.Add("p_UserName", request.UserName);

                param.Add("p_RoleName", request.RoleName);

                param.Add("p_ActionName", request.ActionName);

                param.Add("p_ModuleName", request.ModuleName);

                param.Add("p_TableName", request.TableName);

                param.Add("p_RecordId", request.RecordId);

                param.Add("p_Description", request.Description);

                param.Add("p_OldData", request.OldData);

                param.Add("p_NewData", request.NewData);

                param.Add("p_IpAddress", request.IpAddress);

                param.Add("p_DeviceInfo", request.DeviceInfo);

                var id = await conn.ExecuteScalarAsync<long>("sp_activity_logs", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting activity log");

                Console.WriteLine("DAL ACTIVITY LOG INSERT ERROR: " + ex.Message);

            }

            return response;

        }


    }

}

