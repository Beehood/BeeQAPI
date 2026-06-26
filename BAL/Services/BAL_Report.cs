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
    /// <summary>
    /// Report BAL - Get All Reports
    /// Description:- Validates user role and retrieves report data based on the specified search criteria and filters.
    /// Access:
    /// - Super Admin
    /// - Organization Admin
    /// - Branch Admin
    /// </summary>
    public class BAL_Report : IBAL_Report
    {
        private readonly IDAL_Report _dal;

        public BAL_Report(IDAL_Report dal)
        {
            _dal = dal;
        }

        public async Task<APIGetResponseModel<List<ReportModel>>> GetAll(ReportRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<List<ReportModel>>();

            try
            {
                if (!roles.Contains("Super Admin") &&
                    !roles.Contains("Organization Admin") &&
                    !roles.Contains("Branch Admin"))
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                response = await _dal.GetAll(request,email,transaction);

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);

                return response;
            }
        }
    }
}
