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
    public class BAL_Permission : IBAL_Permission
    {
        private readonly IDAL_Permission _dal;

        public BAL_Permission(IDAL_Permission dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Permission API - Get All Permissions
        /// Author: Swapnlisa
        /// Description:- Fetch permission list with pagination.
        public async Task<APIGetResponseModel<List<PermissionModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<PermissionModel>>()
            {
                Result = new List<PermissionModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("PERMISSION_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (PERMISSION_VIEW required)");
                    return response;
                }

                // ✅ VALIDATION
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
        // CREATE
        // ========================
        /// <summary>
        /// Permission API - Create Permission
        /// Author: Swapnlisa
        /// Description:- Create new permission.
        /// </summary>
        public async Task<APIGetResponseModel<long>> Create(PermissionRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("PERMISSION_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (PERMISSION_CREATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (!string.IsNullOrWhiteSpace(request.PermissionName) && userId != null)
                {
                    response = await _dal.Insert(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (string.IsNullOrWhiteSpace(request.PermissionName))
                        response.ErrorMsgs.Add("Enter Permission Name");

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
        /// Permission API - Update Permission
        /// Author: Swapnlisa
        /// Description:- We use this API to update permission details.
        /// </summary>
        public async Task<APIGetResponseModel<long>> Update(PermissionRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("PERMISSION_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (PERMISSION_UPDATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.PermissionId > 0 &&
                    !string.IsNullOrWhiteSpace(request.PermissionName) &&
                    userId != null)
                {
                    response = await _dal.Update(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (request.PermissionId <= 0)
                        response.ErrorMsgs.Add("Invalid PermissionId");

                    if (string.IsNullOrWhiteSpace(request.PermissionName))
                        response.ErrorMsgs.Add("Enter Permission Name");

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
        /// Permission API - Change Permission Status
        /// Author: Swapnlisa
        /// Description:- We use this API to activate or deactivate a permission.
        /// </summary>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("PERMISSION_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (PERMISSION_STATUS required)");
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

                    if (id <= 0)
                        response.ErrorMsgs.Add("Invalid PermissionId");

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
        /// Permission API - Get Dropdown
        /// Author: Swapnlisa
        /// Description:- Fetch active permission dropdown list.
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
                if (user == null || !user.Permissions.Contains("PERMISSION_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (PERMISSION_VIEW required)");
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
    
