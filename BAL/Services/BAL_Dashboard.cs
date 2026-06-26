using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using System.Data;

namespace BAL.Services
{
    public class BAL_Dashboard : IBAL_Dashboard
    {
        private readonly IDAL_Dashboard _dal;

        public BAL_Dashboard(IDAL_Dashboard dal)
        {
            _dal = dal;
        }

        // ========================
        // DASHBOARD
        // ========================
        /// <summary>
        /// Dashboard BAL - Get Dashboard
        /// Description:- Validates user role and retrieves dashboard statistics and summary information.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<DashboardModel>> GetDashboard(DashboardRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<DashboardModel>();

            try
            {
                // ROLE CHECK

                if (!(roles.Contains("Super Admin")|| roles.Contains("Org Admin")|| roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid user.");
                    return response;
                }

                response = await _dal.GetDashboard(request,roles,email,transaction);
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