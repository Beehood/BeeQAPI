using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Models;
using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class DAL_TimeSlot : IDAL_TimeSlot
    {
        private readonly DBConnection _config;

        public DAL_TimeSlot(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================
        /*TimeSlot DAL - Get All TimeSlots
            Description: Fetches paginated time slot list using stored procedure(multi-result).
            Uses: sp_manage_timeslot
            Action: LIST
            Returns: List<TimeSlotModel> + TotalRecords
            Supports: search + pagination*/
        public async Task<APIGetResponseModel<List<TimeSlotModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<TimeSlotModel>>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "LIST");
                param.Add("p_SlotId", null);
                param.Add("p_BranchId", null);
                param.Add("p_ServiceId", null);
                param.Add("p_DayOfWeek", null);
                param.Add("p_StartTime", null);
                param.Add("p_EndTime", null);
                param.Add("p_MaxCapacity", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_timeslot", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();
                var list = (await multi.ReadAsync<TimeSlotModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching timeslots");
            
            }
            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /*TimeSlot DAL - Get TimeSlot By Id
        Description: Fetches single time slot using SlotId.
         Uses: GETBYID
        Returns: TimeSlotModel*/
        public async Task<APIGetResponseModel<TimeSlotModel>> GetById(long id, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<TimeSlotModel>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "GETBYID");
                param.Add("p_SlotId", id);
                param.Add("p_BranchId", null);
                param.Add("p_ServiceId", null);
                param.Add("p_DayOfWeek", null);
                param.Add("p_StartTime", null);
                param.Add("p_EndTime", null);
                param.Add("p_MaxCapacity", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<TimeSlotModel>("sp_manage_timeslot", param, commandType: CommandType.StoredProcedure);

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
                response.ErrorMsgs.Add("Error while fetching timeslot");

            }
            return response;
        }

        // ========================
        // INSERT
        // ========================
        /*TimeSlot DAL - Insert TimeSlot
        Description: Inserts new time slot record using stored procedure.
        Uses: INSERT
        Returns: newly created SlotId
        Fields: Branch, Service, DayOfWeek, StartTime, EndTime, Capacity*/
        public async Task<APIGetResponseModel<int>> Insert(TimeSlotRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "INSERT");
                param.Add("p_SlotId", null);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_ServiceId", request.ServiceId);
                param.Add("p_DayOfWeek", request.DayOfWeek);
                param.Add("p_StartTime", request.StartTime);
                param.Add("p_EndTime", request.EndTime);
                param.Add("p_MaxCapacity", request.MaxCapacity);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_timeslot", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while inserting timeslot");
              
            }
            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /*TimeSlot DAL - Update TimeSlot
        Description: Updates existing time slot details.
        Uses: UPDATE
         Returns: updated SlotId*/
        public async Task<APIGetResponseModel<int>> Update(TimeSlotRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "UPDATE");
                param.Add("p_SlotId", request.SlotId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_ServiceId", request.ServiceId);
                param.Add("p_DayOfWeek", request.DayOfWeek);
                param.Add("p_StartTime", request.StartTime);
                param.Add("p_EndTime", request.EndTime);
                param.Add("p_MaxCapacity", request.MaxCapacity);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_timeslot", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating timeslot");
             
            }
            return response;
        }

        // ========================
        // STATUS
        // ========================
        /*TimeSlot DAL - Change Status
        Description: Updates time slot status (Active / Inactive).
        Uses: STATUS
        Toggles: 1 ↔ 0
        Returns: SlotId*/
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "STATUS");
                param.Add("p_SlotId", id);
                param.Add("p_BranchId", null);
                param.Add("p_ServiceId", null);
                param.Add("p_DayOfWeek", null);
                param.Add("p_StartTime", null);
                param.Add("p_EndTime", null);
                param.Add("p_MaxCapacity", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_timeslot", param, commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = true;
                response.TotalRecords = 1;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while changing timeslot status");
               
            }
            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        /*TimeSlot DAL - Get TimeSlot Dropdown
        Description: Fetches active time slots for dropdown selection.
        Uses: DROPDOWN
        Returns:
        Id + Name (StartTime - EndTime)
        Used in: Appointment module*/
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(long serviceId, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "DROPDOWN");
                param.Add("p_SlotId", null);
                param.Add("p_BranchId", null);
                param.Add("p_ServiceId", serviceId);
                param.Add("p_DayOfWeek", null);
                param.Add("p_StartTime", null);
                param.Add("p_EndTime", null);
                param.Add("p_MaxCapacity", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_timeslot", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = data.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching timeslot dropdown");
              
            }
            return response;
        }
    }
}        
