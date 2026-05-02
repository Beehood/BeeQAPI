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
        /// Retrieves a paginated list of branches based on the provided pagination request.
        /// </summary>
        /// <param name="request">Pagination details (PageNumber, PageSize)</param>
        /// <param name="user">Authenticated user with permissions</param>
        /// <param name="transaction">Optional database transaction</param>
        /// <returns>List of BranchModel wrapped in API response</returns>
        public async Task<APIGetResponseModel<List<BranchModel>>> GetAll(PaginationRequestDto request,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<BranchModel>>()
            {
                Result = new List<BranchModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCH_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_VIEW)");
                    return response;
                }

                // ✅ VALIDATION (Transaction style)
                if (request != null && request.PageNo > 0 && request.PageSize > 0)
                {
                    response = await _dal.GetAll(request, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.TotalRecords = 0;

                    if (request == null)
                        response.ErrorMsgs.Add("Request cannot be null");

                    if (request?.PageNo <= 0)
                        response.ErrorMsgs.Add("Invalid PageNumber");

                    if (request?.PageSize <= 0)
                        response.ErrorMsgs.Add("Invalid PageSize");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Branch API - Get Branch By Id
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch branch details using BranchId.
        /// Json Request Format Ex- {"BranchId":"1"}
        public async Task<APIGetResponseModel<BranchModel>> GetById(long id,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<BranchModel>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCH_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_VIEW)");
                    return response;
                }

                // ✅ VALIDATION
                if (id > 0)
                {
                    response = await _dal.GetById(id, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = null;

                    if (id <= 0)
                        response.ErrorMsgs.Add("Invalid BranchId");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Branch API - Create Branch
        /// Author: Swapnlisa
        /// Description:- We use this API to create a new branch.
        /// Json Request Format Ex- {"OrganizationId":"1","BranchName":"Main Branch","Address":"BBSR","City":"Bhubaneswar"}
        /// <param name="request">BranchRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns newly created BranchId</returns>
        public async Task<APIGetResponseModel<long>> Create(BranchRequestDto request,string userId,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCH_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_CREATE)");
                    return response;
                }

                // ✅ VALIDATION (Transaction style)
                if (request.OrganizationId > 0 &&
                    !string.IsNullOrWhiteSpace(request.BranchName) &&
                    userId != null)
                {
                    response = await _dal.Insert(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;
                    response.TotalRecords = 0;

                    if (request.OrganizationId <= 0)
                        response.ErrorMsgs.Add("Select Organization");

                    if (string.IsNullOrWhiteSpace(request.BranchName))
                        response.ErrorMsgs.Add("Enter Branch Name");

                    if (userId == null)
                        response.ErrorMsgs.Add("User not authorized");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }
        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// Branch API - Update Branch
        /// Author: Swapnlisa
        /// Description:- We use this API to update existing branch details.
        /// Json Request Format Ex- {"BranchId":"1","OrganizationId":"1","BranchName":"
        public async Task<APIGetResponseModel<long>> Update(BranchRequestDto request,string userId,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCH_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_UPDATE)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.BranchId > 0 &&
                    request.OrganizationId > 0 &&
                    !string.IsNullOrWhiteSpace(request.BranchName) &&
                    userId != null)
                {
                    response = await _dal.Update(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;
                    response.TotalRecords = 0;

                    if (request.BranchId <= 0)
                        response.ErrorMsgs.Add("Invalid BranchId");

                    if (request.OrganizationId <= 0)
                        response.ErrorMsgs.Add("Select Organization");

                    if (string.IsNullOrWhiteSpace(request.BranchName))
                        response.ErrorMsgs.Add("Enter Branch Name");

                    if (userId == null)
                        response.ErrorMsgs.Add("User not authorized");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Branch API - Change Branch Status
        /// Author: Swapnlisa
        /// Description:- We use this API to activate or deactivate a branch
        /// <param name="id">BranchId</param>
        /// <param name="status">0 = Inactive, 1 = Active</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns status update result</returns>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id,int status,long userId,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCH_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (BRANCH_STATUS)");
                    return response;
                }

                // ✅ VALIDATION
                if (id > 0 && (status == 0 || status == 1) && userId > 0)
                {
                    response = await _dal.ChangeStatus(id, status, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;
                    response.TotalRecords = 0;

                    if (id <= 0)
                        response.ErrorMsgs.Add("Invalid BranchId");

                    if (status != 0 && status != 1)
                        response.ErrorMsgs.Add("Invalid Status");

                    if (userId <= 0)
                        response.ErrorMsgs.Add("User not authorized");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }
        // ========================
        // DROPDOWN
        // ========================
        /// <summary>
        /// Branch BAL - Get Branch Dropdown
        /// Author: Swapnlisa
        /// Description:- Fetches active branch dropdown list.
        /// </summary>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>()
            {
                Result = new List<DropdownModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCH_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (BRANCH_VIEW required)");
                    return response;
                }

                response = await _dal.GetDropdown(transaction);
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
