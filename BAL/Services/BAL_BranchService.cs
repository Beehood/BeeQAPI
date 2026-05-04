using BAL.ContractIF.BAL.ContractIF;
using DAL.ContractIF.DAL.ContractIF;
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
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch branch service list with pagination.
        /// Json Request Format Ex- {"PageNo":"1","PageSize":"10"}
        /// <param name="request">PaginationRequestDto</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns paginated branch service list</returns>
        public async Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<BranchServiceModel>>()
            {
                Result = new List<BranchServiceModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCHSERVICE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (BRANCHSERVICE_VIEW required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request != null && request.PageNo > 0 )
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
                Console.WriteLine("BAL ERROR: " + ex.Message);
            }
            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Branch Service API - Get Branch Service By Id
        /// Author: Swapnlisa
        /// Description:- We use this API to fetch branch service details using BranchServiceId.
        /// Json Request Format Ex- {"BranchServiceId":"1"}
        /// </summary>
        /// <param name="id">BranchServiceId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns branch service details</returns>
        public async Task<APIGetResponseModel<BranchServiceModel>> GetById(long id, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<BranchServiceModel>()
            {
                Result = new BranchServiceModel()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCHSERVICE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (BRANCHSERVICE_VIEW required)");
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
                    response.ErrorMsgs.Add("Invalid BranchServiceId");
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
        /// Branch Service API - Create Branch Service
        /// Author: Swapnlisa
        /// Description:- We use this API to create a new branch service mapping.
        /// Json Request Format Ex- {"BranchId":"1","ServiceId":"1","Prefix":"A","DailyLimit":"100"}
        /// </summary>
        /// <param name="request">BranchServiceRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns created BranchServiceId</returns>
        public async Task<APIGetResponseModel<long>> Create(BranchServiceRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCHSERVICE_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (BRANCHSERVICE_CREATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.BranchId > 0 &&
                    request.ServiceId > 0 &&
                    !string.IsNullOrWhiteSpace(request.Prefix) &&
                    request.DailyLimit > 0 &&
                    userId != null)
                {
                    response = await _dal.Insert(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (request.BranchId <= 0)
                        response.ErrorMsgs.Add("Select Branch");

                    if (request.ServiceId <= 0)
                        response.ErrorMsgs.Add("Select Service");

                    if (string.IsNullOrWhiteSpace(request.Prefix))
                        response.ErrorMsgs.Add("Enter Prefix");

                    if (request.DailyLimit <= 0)
                        response.ErrorMsgs.Add("Enter Daily Limit");

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
        /// Branch Service API - Update Branch Service
        /// Author: Swapnlisa
        /// Description:- We use this API to update branch service details.
        /// Json Request Format Ex- {"BranchServiceId":"1","BranchId":"1","ServiceId":"1"}
        /// </summary>
        /// <param name="request">BranchServiceRequestDto</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns updated BranchServiceId</returns>
        public async Task<APIGetResponseModel<long>> Update(BranchServiceRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCHSERVICE_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (BRANCHSERVICE_UPDATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.BranchServiceId > 0 &&
                    request.BranchId > 0 &&
                    request.ServiceId > 0 &&
                    !string.IsNullOrWhiteSpace(request.Prefix) &&
                    request.DailyLimit > 0 &&
                    userId != null)
                {
                    response = await _dal.Update(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (request.BranchServiceId <= 0)
                        response.ErrorMsgs.Add("Invalid BranchServiceId");

                    if (request.BranchId <= 0)
                        response.ErrorMsgs.Add("Select Branch");

                    if (request.ServiceId <= 0)
                        response.ErrorMsgs.Add("Select Service");

                    if (string.IsNullOrWhiteSpace(request.Prefix))
                        response.ErrorMsgs.Add("Enter Prefix");

                    if (request.DailyLimit <= 0)
                        response.ErrorMsgs.Add("Enter Daily Limit");

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
        /// Branch Service API - Change Branch Service Status
        /// Author: Swapnlisa
        /// Description:- We use this API to activate or deactivate branch service.
        /// </summary>
        /// <param name="id">BranchServiceId</param>
        /// <param name="status">0 = Inactive, 1 = Active</param>
        /// <param name="userId">Logged in UserId</param>
        /// <param name="user">TokenUserInfo</param>
        /// <param name="transaction">DB Transaction</param>
        /// <returns>Returns status update result</returns>
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCHSERVICE_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (BRANCHSERVICE_STATUS required)");
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
                        response.ErrorMsgs.Add("Invalid BranchServiceId");

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
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(
            TokenUserInfo user,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>()
            {
                Result = new List<DropdownModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("BRANCHSERVICE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (BRANCHSERVICE_VIEW required)");
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


