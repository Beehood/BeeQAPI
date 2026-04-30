using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_Counter
    {
        Task<APIGetResponseModel<List<CounterModel>>> GetAll(PaginationRequestDto request,TokenUserInfo user,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<CounterModel>> GetById(long id,TokenUserInfo user,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Create(CounterRequestDto request,string userId,TokenUserInfo user,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(CounterRequestDto request,string userId,TokenUserInfo user,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(long id,int status,long userId,TokenUserInfo user,IDbTransaction? transaction = null);
    }
}
