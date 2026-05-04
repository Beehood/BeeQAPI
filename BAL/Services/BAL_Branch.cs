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

    public class BAL_Branch : IBAL_Branch
    {
        private readonly IDAL_Branch _dal;

        public BAL_Branch(IDAL_Branch dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// BAL Layer for Branch Management
        /// Author: Swapnalisa
        /// Description:
        /// Handles business logic, validation, and role-based access control
        /// before calling DAL for database operations.
        public async Task<APIGetResponseModel<List<BranchModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                // All roles can view (filtered in SP)
                return await _dal.GetAll(request, email!, transaction);
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
        /// Fetch branch details by BranchId
        /// Access: All roles (controlled in SP)
        public async Task<APIGetResponseModel<BranchModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
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
        /// Create new branch
        /// Access:
        /// Super Admin
        /// Org Admin
        ///Branch Admin (restricted)
        public async Task<APIGetResponseModel<int>> Create(BranchRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  Branch Admin cannot create
                if (roles.Contains("Branch Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Branch Admin cannot create branch.");
                    return response;
                }

                // VALIDATION
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.BranchName))
                    response.ErrorMsgs.Add("Branch Name is required");

                if (request.OrganizationId <= 0)
                    response.ErrorMsgs.Add("Organization is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                // CALL DAL
                response = await _dal.Insert(request, email, transaction:localtran);

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
        /// Update existing branch
        /// Access:
        /// Super Admin
        /// Org Admin
        /// Branch Admin (restricted)
        public async Task<APIGetResponseModel<int>> Update(BranchRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  Branch Admin cannot update
                if (roles.Contains("Branch Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Branch Admin cannot update branch.");
                    return response;
                }

                if (request == null || request.BranchId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid branch data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.BranchName))
                    response.ErrorMsgs.Add("Branch Name is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                response = await _dal.Update(request, email, localtran);

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
        // CHANGE STATUS (DELETE)
        // ========================
        /// <summary>
        /// Activate/Deactivate (Soft Delete) Branch
        /// Access:
        /// Super Admin only
        /// Org Admin
        ///Branch Admin
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // Only Super Admin can delete
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can delete branch.");
                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid branch ID.");
                    return response;
                }

                response = await _dal.ChangeStatus(id, email, localtran);

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