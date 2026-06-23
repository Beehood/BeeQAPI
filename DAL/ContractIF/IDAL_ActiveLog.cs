using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
   
        public interface IDAL_ActiveLog
    {
            Task<APIGetResponseModel<List<ActivityLogModel>>> GetAll(PaginationRequestDto request,string email,IDbTransaction? transaction = null);

            Task<APIGetResponseModel<ActivityLogModel>> GetById(long id,string email,IDbTransaction? transaction = null);

            Task<APIGetResponseModel<int>> Insert(ActivityLogRequestDto request,string email,IDbTransaction? transaction = null);
        }

    }
