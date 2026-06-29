using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_NotificationLog
    {
        Task<APIGetResponseModel<List<NotificationLogModel>>> GetAll(PaginationRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<NotificationLogModel>> GetById(long id,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(NotificationLogRequestDto request,string email,IDbTransaction? transaction = null);
    }
}
