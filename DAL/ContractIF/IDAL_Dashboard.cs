using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Dashboard
    {
            Task<APIGetResponseModel<DashboardModel>> GetDashboard(DashboardRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);
        
    }
}
