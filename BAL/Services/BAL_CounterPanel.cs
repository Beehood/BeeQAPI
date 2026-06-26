using BAL.ContractIF;
using DAL.ContractIF;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class BAL_CounterPanel : IBAL_CounterPanel
    {
        private readonly IDAL_CounterPanel _dal;

        private readonly ILogger<BAL_CounterPanel> _logger;

        public BAL_CounterPanel(IDAL_CounterPanel dal,ILogger<BAL_CounterPanel> logger)
        {
            _dal = dal;

            _logger = logger;
        }

        // ========================
        // DASHBOARD
        // ========================
        /// <summary>
        /// Counter Panel BAL - Get Dashboard
        /// Description:- Validates user role and retrieves the counter panel dashboard information.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// - Branch User
        /// </summary>

        public async Task<APIGetResponseModel<CounterPanelDashboardModel>>GetDashboard(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<CounterPanelDashboardModel>();

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                response =await _dal.GetDashboard(request,email,transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                _logger.LogError(ex,"BAL GetDashboard Error");

                response.ErrorMsgs.Add("Error while loading dashboard");
            }

            return response;
        }

        // ========================
        // CALL NEXT TOKEN
        // ========================
        /// <summary>
        /// Counter Panel BAL - Call Next Token
        /// Description:- Validates user role and counter details before calling the next available token.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// - Branch User
        /// </summary>

        public async Task<APIGetResponseModel<CallNextTokenResponseDto>>CallNextToken(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<CallNextTokenResponseDto>();

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                if (request.CounterId <= 0)
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Counter required.");

                    return response;
                }

                response =await _dal.CallNextToken(request,email,transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);

                _logger.LogError(ex,"DAL CALL NEXT TOKEN ERROR");
            }

            return response;
        }

        // ========================
        // START SERVICE
        // ========================
        /// <summary>
        /// Counter Panel BAL - Start Service
        /// Description:- Validates user role and starts service for the currently called token.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// - Branch User
        /// </summary>
        public async Task<APIGetResponseModel<int>>StartService(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<int>();

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                response =await _dal.StartService(request,email,transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                _logger.LogError(ex,"BAL StartService Error");

                response.ErrorMsgs.Add("Error while starting service");
            }

            return response;
        }

        // ========================
        // COMPLETE SERVICE
        // ========================
        /// <summary>
        /// Counter Panel BAL - Complete Service
        /// Description:- Validates user role and marks the current token service as completed.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// - Branch User
        /// </summary>
        public async Task<APIGetResponseModel<int>>CompleteService(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<int>();

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                response =await _dal.CompleteService(request,email,transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                _logger.LogError(ex,"BAL CompleteService Error");

                response.ErrorMsgs.Add("Error while completing service");
            }

            return response;
        }

        // ========================
        // SKIP TOKEN
        // ========================
        /// <summary>
        /// Counter Panel BAL - Skip Token
        /// Description:- Validates user role and skips the currently assigned token.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// - Branch User
        /// </summary>
        public async Task<APIGetResponseModel<int>>SkipToken(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<int>();

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                response =await _dal.SkipToken(request,email,transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                _logger.LogError(ex,"BAL SkipToken Error");

                response.ErrorMsgs.Add("Error while skipping token");
            }

            return response;
        }

        // ========================
        // RECALL TOKEN
        // ========================
        /// <summary>
        /// Counter Panel BAL - Recall Token
        /// Description:- Validates user role and recalls the previously called token for service.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// - Branch User
        /// </summary>
        public async Task<APIGetResponseModel<int>>RecallToken(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<int>();

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                response =await _dal.RecallToken(request,email,transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                _logger.LogError(ex,"BAL RecallToken Error");

                response.ErrorMsgs.Add("Error while recalling token");
            }

            return response;
        }
    }
}
