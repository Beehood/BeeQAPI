using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_CounterPanel
    {
        Task<APIGetResponseModel<CounterPanelDashboardModel>>GetDashboard(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<CallNextTokenResponseDto>>CallNextToken(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>>StartService(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>>CompleteService(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>>SkipToken(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>>RecallToken(CounterPanelActionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);
    }
}
