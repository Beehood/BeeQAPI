using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_NotificationLog
    {
        Task<APIGetResponseModel<List<NotificationLogModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<NotificationLogModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(NotificationLogRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);
    }
}