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
        /// Retrieves a paginated list of counters based on the provided pagination request.
        /// </summary>
        /// <param name="request">Pagination details (PageNumber, PageSize)</param>
        /// <param name="user">Authenticated user with permissions</param>
        /// <param name="transaction">Optional database transaction</param>
        /// <returns>List of CounterModel wrapped in API response</returns>
        public async Task<APIGetResponseModel<List<CounterModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<CounterModel>>()
            {
                Result = new List<CounterModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTER_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (COUNTER_VIEW)");
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
        /// Counter API - Get Counter By Id
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch counter details using CounterId.
        /// Json Request Format Ex- {"CounterId":"1"}
        /// </summary>
        public async Task<APIGetResponseModel<CounterModel>> GetById(long id, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<CounterModel>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTER_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (COUNTER_VIEW)");
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
                        response.ErrorMsgs.Add("Invalid CounterId");
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
        /// Counter API - Create Counter
        /// Author: Swapnlisa
        /// Description:- We use this API to create a new counter.
        /// Json Request Format Ex- {"BranchId":"1","CounterName":"Counter 1","CounterCode":"C001"}
        /// </summary>
        public async Task<APIGetResponseModel<long>> Create(CounterRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTER_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (COUNTER_CREATE)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.BranchId > 0 &&
                    !string.IsNullOrWhiteSpace(request.CounterName) &&
                    userId != null)
                {
                    response = await _dal.Insert(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;
                    response.TotalRecords = 0;

                    if (request.BranchId <= 0)
                        response.ErrorMsgs.Add("Select Branch");

                    if (string.IsNullOrWhiteSpace(request.CounterName))
                        response.ErrorMsgs.Add("Enter Counter Name");

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
        /// Counter API - Update Counter
        /// Author: Swapnlisa
        /// Description:- We use this API to update existing counter details.
        /// Json Request Format Ex- {"CounterId":"1","BranchId":"1","CounterName":"Updated Counter"}
        /// </summary>
        public async Task<APIGetResponseModel<long>> Update(CounterRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTER_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (COUNTER_UPDATE)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.CounterId > 0 &&
                    request.BranchId > 0 &&
                    !string.IsNullOrWhiteSpace(request.CounterName) &&
                    userId != null)
                {
                    response = await _dal.Update(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;
                    response.TotalRecords = 0;

                    if (request.CounterId <= 0)
                        response.ErrorMsgs.Add("Invalid CounterId");

                    if (request.BranchId <= 0)
                        response.ErrorMsgs.Add("Select Branch");

                    if (string.IsNullOrWhiteSpace(request.CounterName))
                        response.ErrorMsgs.Add("Enter Counter Name");

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
        /// Counter API - Change Counter Status
        /// Author: Swapnlisa
        /// Description:- We use this API to activate or deactivate a counter
        /// </summary>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTER_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized (COUNTER_STATUS)");
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
                        response.ErrorMsgs.Add("Invalid CounterId");

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