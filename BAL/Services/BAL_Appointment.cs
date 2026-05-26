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
    public class BAL_Appointment : IBAL_Appointment
    {
        private readonly IDAL_Appointment _dal;
        public BAL_Appointment(IDAL_Appointment dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Appointment API - Get All Appointments
        /// Description:- Fetch appointment list with pagination & role validation.
        /// Access:- Super Admin, Org Admin, Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<List<AppointmentModel>>> GetAll(AppointmentRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") || roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<AppointmentModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }
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
        /// Appointment API - Get Appointment By Id
        /// Description:- Fetch appointment details using AppointmentId.
        /// Access:- Super Admin, Org Admin, Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<AppointmentModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") || roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<AppointmentModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }
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
        /// Appointment API - Create Appointment
        /// Description:- Creates new appointment with validation.
        /// Access:- Super Admin, Org Admin, Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Create(AppointmentRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;
            try
            {
                if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") || roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (request.OrganizationId <= 0) response.ErrorMsgs.Add("Organization is required");
                if (request.BranchId <= 0) response.ErrorMsgs.Add("Branch is required");
                if (request.ServiceId <= 0) response.ErrorMsgs.Add("Service is required");
                if (string.IsNullOrWhiteSpace(request.CustomerName)) response.ErrorMsgs.Add("Customer name is required");
                if (string.IsNullOrWhiteSpace(request.CustomerPhone)) response.ErrorMsgs.Add("Customer phone is required");
                if (request.AppointmentDate == null) response.ErrorMsgs.Add("Appointment date is required");

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
        /// Appointment API - Update Appointment
        /// Description:- Updates appointment details with validation.
        /// Access:- Super Admin, Org Admin, Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Update(AppointmentRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;
            try
            {
                if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") || roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request == null || request.AppointmentId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid appointment data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.CustomerName)) response.ErrorMsgs.Add("Customer name is required");
                if (string.IsNullOrWhiteSpace(request.CustomerPhone)) response.ErrorMsgs.Add("Customer phone is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
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
        /// Appointment API - Change Appointment Status
        /// Description:- Updates appointment status.
        /// Access:- Super Admin, Org Admin, Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, int status, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;
            try
            {
                if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") || roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid appointment ID.");
                    return response;
                }

                response = await _dal.ChangeStatus(id, status, email, transaction: localtran);

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
    }
}
