using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
   
        public interface IDAL_Report
        {
            Task<APIGetResponseModel<List<ReportModel>>> GetAll(ReportRequestDto request,string email,IDbTransaction? transaction = null);
        }
    

}
