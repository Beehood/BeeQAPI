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

    public class DAL_Appointment : IDAL_Appointment

    {

        private readonly DBConnection _config;

        public DAL_Appointment(DBConnection config)

        {

            _config = config;

        }

        // ========================

        // GET ALL

        // ========================

        /// <summary>

        /// Appointment DAL - Get All Appointments

        /// Description:- Fetch paginated appointment list with search support.

        /// Returns total count + list (multi-result).

        /// </summary>

        public async Task<APIGetResponseModel<List<AppointmentModel>>> GetAll(AppointmentRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<AppointmentModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_AppointmentId", null);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_ServiceId", null);

                param.Add("p_UserId", null);

                param.Add("p_CustomerName", null);

                param.Add("p_CustomerPhone", null);

                param.Add("p_AppointmentDate", null);

                param.Add("p_TimeSlotId", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_appointment", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<AppointmentModel>()).ToList();

                response.Result = list;

                response.IsSuccess = list.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching appointments");

                Console.WriteLine("DAL GET ALL ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// Appointment DAL - Get Appointment By Id

        /// Description:- Fetch single appointment details using AppointmentId.

        /// </summary>

        public async Task<APIGetResponseModel<AppointmentModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<AppointmentModel>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_AppointmentId", id);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_ServiceId", null);

                param.Add("p_UserId", null);

                param.Add("p_CustomerName", null);

                param.Add("p_CustomerPhone", null);

                param.Add("p_AppointmentDate", null);

                param.Add("p_TimeSlotId", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<AppointmentModel>("sp_manage_appointment", param, commandType: CommandType.StoredProcedure);

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

                response.ErrorMsgs.Add("Error while fetching appointment");

                Console.WriteLine("DAL GET BY ID ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// Appointment DAL - Insert Appointment

        /// Description:- Inserts new appointment record using stored procedure.

        /// Returns newly created AppointmentId.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(AppointmentRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_AppointmentId", null);

                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_ServiceId", request.ServiceId);

                param.Add("p_UserId", request.UserId);

                param.Add("p_CustomerName", request.CustomerName);

                param.Add("p_CustomerPhone", request.CustomerPhone);

                param.Add("p_AppointmentDate", request.AppointmentDate);

                param.Add("p_TimeSlotId", request.TimeSlotId);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_appointment", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting appointment");

                Console.WriteLine("DAL INSERT ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // UPDATE

        // ========================

        /// <summary>

        /// Appointment DAL - Update Appointment

        /// Description:- Updates existing appointment details.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Update(AppointmentRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_AppointmentId", request.AppointmentId);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_ServiceId", request.ServiceId);

                param.Add("p_UserId", request.UserId);

                param.Add("p_CustomerName", request.CustomerName);

                param.Add("p_CustomerPhone", request.CustomerPhone);

                param.Add("p_AppointmentDate", request.AppointmentDate);

                param.Add("p_TimeSlotId", request.TimeSlotId);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_appointment", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while updating appointment");

                Console.WriteLine("DAL UPDATE ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // CHANGE STATUS

        // ========================

        /// <summary>

        /// Appointment DAL - Change Status

        /// Description:- Updates appointment status (Booked/Completed/Cancelled).

        /// </summary>

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, int status, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_AppointmentId", id);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_ServiceId", null);

                param.Add("p_UserId", null);

                param.Add("p_CustomerName", null);

                param.Add("p_CustomerPhone", null);

                param.Add("p_AppointmentDate", null);

                param.Add("p_TimeSlotId", null);

                param.Add("p_Status", status);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_appointment", param, commandType: CommandType.StoredProcedure);

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


