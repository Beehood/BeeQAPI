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
    public class BAL_CounterService : IBAL_CounterService
    {
        private readonly IDAL_CounterService _dal;

        public BAL_CounterService(IDAL_CounterService dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<CounterServiceModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<CounterServiceModel>>()
            {
                Result = new List<CounterServiceModel>()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTERSERVICE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (COUNTERSERVICE_VIEW required)");
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
        public async Task<APIGetResponseModel<CounterServiceModel>> GetById(long id, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<CounterServiceModel>()
            {
                Result = new CounterServiceModel()
            };

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTERSERVICE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (COUNTERSERVICE_VIEW required)");
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
                    response.ErrorMsgs.Add("Invalid CounterServiceId");
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
        public async Task<APIGetResponseModel<long>> Create(CounterServiceRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTERSERVICE_CREATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (COUNTERSERVICE_CREATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.CounterId > 0 &&
                    request.ServiceId > 0 &&
                    userId != null)
                {
                    response = await _dal.Insert(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (request.CounterId <= 0)
                        response.ErrorMsgs.Add("Select Counter");

                    if (request.ServiceId <= 0)
                        response.ErrorMsgs.Add("Select Service");

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
        public async Task<APIGetResponseModel<long>> Update(CounterServiceRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                // 🔐 RBAC
                if (user == null || !user.Permissions.Contains("COUNTERSERVICE_UPDATE"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (COUNTERSERVICE_UPDATE required)");
                    return response;
                }

                // ✅ VALIDATION
                if (request.CounterServiceId > 0 &&
                    request.CounterId > 0 &&
                    request.ServiceId > 0 &&
                    userId != null)
                {
                    response = await _dal.Update(request, userId, transaction);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = 0;

                    if (request.CounterServiceId <= 0)
                        response.ErrorMsgs.Add("Invalid CounterServiceId");

                    if (request.CounterId <= 0)
                        response.ErrorMsgs.Add("Select Counter");

                    if (request.ServiceId <= 0)
                        response.ErrorMsgs.Add("Select Service");

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
                if (user == null || !user.Permissions.Contains("COUNTERSERVICE_STATUS"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (COUNTERSERVICE_STATUS required)");
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
                        response.ErrorMsgs.Add("Invalid CounterServiceId");

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
                if (user == null || !user.Permissions.Contains("COUNTERSERVICE_VIEW"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access (COUNTERSERVICE_VIEW required)");
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
