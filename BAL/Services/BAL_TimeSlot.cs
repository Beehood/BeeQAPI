using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class BAL_TimeSlot : IBAL_TimeSlot
    {
        private readonly IDAL_TimeSlot _dal;

        public BAL_TimeSlot(IDAL_TimeSlot dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Time Slot BAL - Get All Time Slots
        /// Description:- Retrieves the list of available time slots based on the specified search criteria.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<List<TimeSlotModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
       
        {
            try
            {
                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetAll", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Time Slot BAL - Get Time Slot By Id
        /// Description:- Retrieves the details of a specific time slot.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<TimeSlotModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            try
            {
                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetById", ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Time Slot BAL - Create Time Slot
        /// Description:- Validates request data before creating a new time slot.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Create(TimeSlotRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (request.BranchId <= 0)
                    response.ErrorMsgs.Add("Branch required");

                if (request.ServiceId <= 0)
                    response.ErrorMsgs.Add("Service required");

                if (request.StartTime == null || request.EndTime == null)
                    response.ErrorMsgs.Add("Time required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                response = await _dal.Insert(request, email, transaction: localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// Time Slot BAL - Update Time Slot
        /// Description:- Validates request data before updating an existing time slot.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Update(TimeSlotRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (request == null || request.SlotId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid timeslot data.");
                    return response;
                }

                response = await _dal.Update(request, email, transaction: localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // STATUS
        // ========================
        /// <summary>
        /// Time Slot BAL - Change Time Slot Status
        /// Description:- Changes the active or inactive status of the specified time slot.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid Slot ID.");
                    return response;
                }

                response = await _dal.ChangeStatus(id, email, transaction: localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        /// <summary>
        /// Time Slot BAL - Get Time Slot Dropdown
        /// Description:- Retrieves available time slot dropdown values for the selected service.
        /// Access:
        /// - Authenticated Users
        /// </summary>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(long serviceId, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                response = await _dal.GetDropdown(serviceId, email, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }
    }
}
