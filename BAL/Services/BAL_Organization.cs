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


    public class BAL_Organization : IBAL_Organization
    {
        private readonly IDAL_Organization _dal;

        public BAL_Organization(IDAL_Organization dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Organization API - Get All Organizations
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch organization list with pagination.
        /// Json Request Format Ex- {"PageNumber":"1","PageSize":"10"}
        /// </summary>
        /// <param name="request">PaginationRequestDto</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns paginated organization list</returns>
        public async Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(PaginationRequestDto request,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<OrganizationModel>>()
            {
                Result = new List<OrganizationModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ORG_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_VIEW required)");
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
        // GET BY ID
        // ========================
        /// <summary>
        /// Organization API - Get Organization By Id
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch organization details using OrganizationId.
        /// Json Request Format Ex- {"OrganizationId":"1"}
        /// </summary>
        /// <param name="id">OrganizationId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns organization details</returns>
        public async Task<APIGetResponseModel<OrganizationModel>> GetById(long id,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<OrganizationModel>()
            {
                Result = new OrganizationModel()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ORG_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_VIEW required)");
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
                    response.ErrorMsgs.Add("Invalid OrganizationId");
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
        /// Organization API - Create Organization
        /// Author: Swapnlisa
        /// Description:- We use this API to create a new organization.
        /// Json Request Format Ex- {"OrganizationName":"ABC Pvt Ltd","Address":"BBSR"}
        /// </summary>
        /// <param name="request">OrganizationRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns created OrganizationId</returns>
        public async Task<APIGetResponseModel<long>> Create(OrganizationRequestDto request,string userId,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ORG_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_CREATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (!string.IsNullOrWhiteSpace(request.Name) && userId != null)
                {
                    response = await _dal.Insert(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (string.IsNullOrWhiteSpace(request.Name))
                        response.ErrorMsgs.Add("Enter Organization Name");

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
        /// Organization API - Update Organization
        /// Author: Swapnlisa
        /// Description:- We use this API to update organization details.
        /// Json Request Format Ex- {"OrganizationId":"1","OrganizationName":"Updated Name"}
        /// </summary>
        /// <param name="request">OrganizationRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns updated OrganizationId</returns>
        public async Task<APIGetResponseModel<long>> Update(OrganizationRequestDto request,string userId,TokenUserInfo user,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("ORG_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_UPDATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.OrganizationId > 0 &&
                    !string.IsNullOrWhiteSpace(request.Name) &&
                    userId != null)
                {
                    response = await _dal.Update(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (request.OrganizationId <= 0)
                        response.ErrorMsgs.Add("Invalid OrganizationId");

                    if (string.IsNullOrWhiteSpace(request.Name))
                        response.ErrorMsgs.Add("Enter Organization Name");

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
        /// Organization API - Change Organization Status
        /// Author: Swapnlisa
        /// Description:- We use this API to activate or deactivate an organization.
        /// </summary>
        /// <param name="id">OrganizationId</param>
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
                if (user == null || !user.Permissions.Contains("ORG_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (ORG_STATUS required)");
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
                        response.ErrorMsgs.Add("Invalid OrganizationId");

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
    }
}