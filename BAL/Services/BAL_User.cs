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
    public class BAL_User : IBAL_User
    {
        private readonly IDAL_User _dal;

        public BAL_User(IDAL_User dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// User BAL - Get All Users
        /// Author: Swapnlisa
        /// Description:- Fetches paginated user list with RBAC validation.

        public async Task<APIGetResponseModel<List<UserModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<UserModel>>()
            {
                Result = new List<UserModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("USER_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (USER_VIEW required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request != null && request.PageNo > 0)
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

                    //if (request?.PageSize <= 0)
                    //    response.ErrorMsgs.Add("Invalid PageSize");
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
        /// User BAL - Get User By Id
        /// Author: Swapnlisa
        /// Description:- Fetches single user based on UserId.
        public async Task<APIGetResponseModel<UserModel>> GetById(long id, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<UserModel>()
            {
                Result = new UserModel()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("USER_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (USER_VIEW required)");
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
                    response.ErrorMsgs.Add("Invalid UserId");
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
        /// User BAL - Create User
        /// Author: Swapnlisa
        /// Description:- Validates and inserts new user.
        public async Task<APIGetResponseModel<long>> Create(UserRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("USER_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (USER_CREATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (!string.IsNullOrEmpty(request.Name) &&
                    !string.IsNullOrEmpty(request.Email) &&
                    request.RoleId > 0 &&
                    userId != null)
                {
                    response = await _dal.Insert(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (string.IsNullOrEmpty(request.Name))
                        response.ErrorMsgs.Add("Name is required");

                    if (string.IsNullOrEmpty(request.Email))
                        response.ErrorMsgs.Add("Email is required");

                    if (request.RoleId <= 0)
                        response.ErrorMsgs.Add("Select Role");

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
        /// User BAL - Update User
        /// Author: Swapnlisa
        /// Description:- Updates user details.
        public async Task<APIGetResponseModel<long>> Update(UserRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("USER_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (USER_UPDATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.UserId > 0 &&
                    !string.IsNullOrEmpty(request.Name) &&
                    !string.IsNullOrEmpty(request.Email) &&
                    request.RoleId > 0 &&
                    userId != null)
                {
                    response = await _dal.Update(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (request.UserId <= 0)
                        response.ErrorMsgs.Add("Invalid UserId");

                    if (string.IsNullOrEmpty(request.Name))
                        response.ErrorMsgs.Add("Name is required");

                    if (string.IsNullOrEmpty(request.Email))
                        response.ErrorMsgs.Add("Email is required");

                    if (request.RoleId <= 0)
                        response.ErrorMsgs.Add("Select Role");

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
        /// User BAL - Dropdown
        /// Author: Swapnlisa
        /// Description:- Fetches active users for dropdown.
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("USER_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (USER_STATUS required)");
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
                        response.ErrorMsgs.Add("Invalid UserId");

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
                if (user == null || !user.Permissions.Contains("USER_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (USER_VIEW required)");
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

