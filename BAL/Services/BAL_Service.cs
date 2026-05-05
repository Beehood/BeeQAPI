using BAL.ContractIF;
using BAL.ContractIF;
using DAL.ContractIF;
using Dapper;
using Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace BAL.Implementation
{
    public class BAL_Service : IBAL_Service
    {
        private readonly IDAL_Service _dal;

        public BAL_Service(IDAL_Service dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Fetch all services (paginated)
        /// Access: Super Admin, Org Admin, Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<List<ServiceModel>>> GetAll(PaginationRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                //  Role validation (same pattern as Organization)
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<ServiceModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetAll (Service)", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Fetch service by ID
        /// Access: Super Admin, Org Admin, Branch Admin
        public async Task<APIGetResponseModel<ServiceModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                // Role validation added
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<ServiceModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetById (Service)", ex);
            }
        }
        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Create new service
        /// Access:
        /// Super Admin
        /// Org Admin
        /// Branch Admin ✅ (Allowed)
        public async Task<APIGetResponseModel<int>> Create(ServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // 🔥 ROLE CHECK
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

                if (string.IsNullOrWhiteSpace(request.ServiceName))
                    response.ErrorMsgs.Add("Service Name is required");

                if (request.OrganizationId <= 0)
                    response.ErrorMsgs.Add("Organization is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                // CALL DAL
                response = await _dal.Insert(request, email, localtran);

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
        /// Update service
        /// Access:
        /// Super Admin
        /// Org Admin
        /// Branch Admin ✅ (Allowed)
    
        public async Task<APIGetResponseModel<int>> Update(ServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  ROLE CHECK
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                //  VALIDATION
                if (request == null || request.ServiceId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid service data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.ServiceName))
                    response.ErrorMsgs.Add("Service Name is required");

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
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Activate / Deactivate service
        /// Access:
        /// Super Admin
        /// Org Admin
        /// Branch Admin ✅ (Allowed)
 
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  ROLE CHECK
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
                    response.ErrorMsgs.Add("Invalid service ID.");
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
        /// Fetch service dropdown
        /// Access: All roles
       
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                // ROLE CHECK (optional - keep if needed)
                // if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") || roles.Contains("Branch Admin")))
                // {
                //     response.IsSuccess = false;
                //     response.ErrorMsgs.Add("Access denied.");
                //     return response;
                // }

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