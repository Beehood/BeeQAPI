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
    public class BAL_CounterService : IBAL_CounterService
    {
        private readonly IDAL_CounterService _dal;

        public BAL_CounterService(IDAL_CounterService dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Counter Service BAL - Get All Counter Services
        /// Description:- Validates user role and retrieves the list of counter service mappings.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<List<CounterServiceModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<CounterServiceModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetAll (CounterService)", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Counter Service BAL - Get Counter Service By Id
        /// Description:- Validates user role and retrieves counter service mapping details.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<CounterServiceModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<CounterServiceModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetById (CounterService)", ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Counter Service BAL - Create Counter Service
        /// Description:- Validates user role and request data before creating a new counter service mapping.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Create(CounterServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // ROLE CHECK
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                // VALIDATION
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (request.CounterId <= 0)
                    response.ErrorMsgs.Add("Counter is required");

                if (request.BranchServiceId <= 0)
                    response.ErrorMsgs.Add("BranchService is required");

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
        /// Counter Service BAL - Update Counter Service
        /// Description:- Validates user role and request data before updating an existing counter service mapping.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Update(CounterServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // ROLE CHECK
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request == null || request.CounterServiceId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid counter service data.");
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
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Counter Service BAL - Change Counter Service Status
        /// Description:- Validates user role and changes the active/inactive status of a counter service mapping.
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
                // ROLE CHECK
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid CounterService ID.");
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
        /// Counter Service BAL - Get Counter Service Dropdown
        /// Description:- Retrieves counter service dropdown values for UI selection controls.
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

