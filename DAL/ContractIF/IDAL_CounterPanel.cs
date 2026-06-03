using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_CounterPanel
    {
        Task<APIGetResponseModel<CounterPanelDashboardModel>>GetDashboard(CounterPanelActionRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<CallNextTokenResponseDto>>CallNextToken(CounterPanelActionRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>>StartService( CounterPanelActionRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>>CompleteService(CounterPanelActionRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>>SkipToken(CounterPanelActionRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>>RecallToken(CounterPanelActionRequestDto request,string email,IDbTransaction? transaction = null);
    }
}
