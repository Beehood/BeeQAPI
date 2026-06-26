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
    public class BAL_Counter : IBAL_Counter
    {
        private readonly IDAL_Counter _dal;

        public BAL_Counter(IDAL_Counter dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Counter BAL - Get All Counters
        /// Description:- Validates user role and retrieves the list of counters.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<List<CounterModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                //  ROLE CHECK
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<CounterModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in Counter GetAll", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Counter BAL - Get Counter By Id
        /// Description:- Validates user role and retrieves counter details.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<CounterModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<CounterModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in Counter GetById", ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Counter BAL - Create Counter
        /// Description:- Validates user role and request data before creating a new counter.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Create(CounterRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  ROLE CHECK
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                //  VALIDATION
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.CounterName))
                    response.ErrorMsgs.Add("Counter Name is required");

                if (request.BranchId <= 0)
                    response.ErrorMsgs.Add("Branch is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                // CALL DAL
                response = await _dal.Insert(request, email, transaction: localtran);

                if (transaction == null && localtran != null)localtran.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null && localtran != null)localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// Counter BAL - Update Counter
        /// Description:- Validates user role and request data before updating an existing counter.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
       
        public async Task<APIGetResponseModel<int>> Update(CounterRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request == null || request.CounterId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid counter data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.CounterName))
                    response.ErrorMsgs.Add("Counter Name is required");

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
        /// Counter BAL - Change Counter Status
        /// Description:- Validates user role and changes the active/inactive status of a counter.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid counter ID.");
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
        /// Counter BAL - Get Counter Dropdown
        /// Description:- Retrieves counter dropdown values for UI selection controls.
        /// Access:
        /// - Authenticated Users
        /// </summary>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                response = await _dal.GetDropdown(email, transaction);
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