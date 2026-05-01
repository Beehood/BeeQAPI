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
    public class BAL_Role : IBAL_Role
    {
        private readonly IDAL_Role _dal;

        public BAL_Role(IDAL_Role dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Retrieves a paginated list of roles.
        /// </summary>
        public async Task<APIGetResponseModel<List<RoleModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<RoleModel>>()
            {
                Result = new List<RoleModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ROLE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (ROLE_VIEW)");
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
        public async Task<APIGetResponseModel<RoleModel>> GetById(long id, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<RoleModel>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ROLE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (ROLE_VIEW)");
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
                    response.ErrorMsgs.Add("Invalid RoleId");
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
        public async Task<APIGetResponseModel<long>> Create(RoleRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ROLE_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (ROLE_CREATE)");
                    return response;
                }

                // ✅ VALIDATION
                if (!string.IsNullOrWhiteSpace(request.RoleName) &&
                    request.OrganizationId > 0 &&
                    userId != null)
                {
                    response = await _dal.Insert(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;

                    if (string.IsNullOrWhiteSpace(request.RoleName))
                        response.ErrorMsgs.Add("Enter Role Name");

                    if (request.OrganizationId <= 0)
                        response.ErrorMsgs.Add("Select Organization");

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
        public async Task<APIGetResponseModel<long>> Update(RoleRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ROLE_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (ROLE_UPDATE)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.RoleId > 0 &&
                    !string.IsNullOrWhiteSpace(request.RoleName) &&
                    request.OrganizationId > 0 &&
                    userId != null)
                {
                    response = await _dal.Update(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;

                    if (request.RoleId <= 0)
                        response.ErrorMsgs.Add("Invalid RoleId");

                    if (string.IsNullOrWhiteSpace(request.RoleName))
                        response.ErrorMsgs.Add("Enter Role Name");

                    if (request.OrganizationId <= 0)
                        response.ErrorMsgs.Add("Select Organization");

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
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ROLE_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (ROLE_STATUS)");
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

                    if (id <= 0)
                        response.ErrorMsgs.Add("Invalid RoleId");

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
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>()
            {
                Result = new List<DropdownModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ROLE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (ROLE_VIEW)");
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

