using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using System.Data;

namespace BAL.Implementation
{
    public class BAL_BranchService : IBAL_BranchService
    {
        private readonly IDAL_BranchService _dal;


        public BAL_BranchService(IDAL_BranchService dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Branch Service API - Get All Branch Services
        /// Author: Swapnalisa
        /// Description: Fetch branch service list with pagination.
        /// Access: Super Admin, Org Admin, Branch Admin
        public async Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<BranchServiceModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }
                //  All roles allowed (filtered in SP)
                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetAll (BranchService)", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Branch Service API - Get By Id
        /// Description: Fetch branch service details by ID.
        /// Access: Super Admin, Org Admin, Branch Admin
        public async Task<APIGetResponseModel<BranchServiceModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<BranchServiceModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetById (BranchService)", ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Create new Branch Service
        /// Access:
        /// Super Admin
        /// Org Admin
        /// Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Create(BranchServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // ROLE CHECK (All 3 allowed)
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
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

                if (request.BranchId <= 0)
                    response.ErrorMsgs.Add("Branch is required");

                if (request.ServiceId <= 0)
                    response.ErrorMsgs.Add("Service is required");

                if (string.IsNullOrWhiteSpace(request.Prefix))
                    response.ErrorMsgs.Add("Prefix is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                // CALL DAL
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
        /// Update Branch Service
        /// Access:
        /// Super Admin
        /// Org Admin
        /// Branch Admin
        public async Task<APIGetResponseModel<int>> Update(BranchServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  ROLE CHECK (All 3 allowed)
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request == null || request.BranchServiceId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid branch service data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Prefix))
                    response.ErrorMsgs.Add("Prefix is required");

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
        /// Activate / Deactivate Branch Service
        /// Access:
        /// Super Admin
        /// Org Admin
        /// Branch Admin 
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  ROLE CHECK (All 3 allowed)
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
                    response.ErrorMsgs.Add("Invalid BranchService ID.");
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

        public async Task<
        APIGetResponseModel<List<DropdownModel>>>
        GetDropdown(
            string email,
            IDbTransaction? transaction = null
        )
        {
            var response =
                new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                response =
                    await _dal.GetDropdown(
                        email,
                        transaction
                    );
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // BRANCH DROPDOWN BY ORGANIZATION
        // ========================

        public async Task<
        APIGetResponseModel<List<DropdownModel>>>
        GetBranchDropdownByOrganization(
            long orgId,
            string email,
            IDbTransaction? transaction = null
        )
        {
            var response =
                new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                response =
                    await _dal.GetBranchDropdownByOrganization(
                        orgId,
                        email,
                        transaction
                    );
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