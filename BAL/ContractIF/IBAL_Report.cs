using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
  
        public interface IBAL_Report
        {
            Task<APIGetResponseModel<List<ReportModel>>> GetAll(ReportRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null);
        }
    
}