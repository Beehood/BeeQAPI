using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_ActiveLog
    {
        Task<APIGetResponseModel<List<ActivityLogModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<ActivityLogModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(ActivityLogRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);
    }
}