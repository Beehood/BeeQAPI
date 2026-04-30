using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
   
    public interface IDAL_Counter
    {
        Task<APIGetResponseModel<List<CounterModel>>> GetAll(PaginationRequestDto request,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<CounterModel>> GetById(long id,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Insert(CounterRequestDto request,string userId,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(CounterRequestDto request,string userId,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(long id,int status,long userId,IDbTransaction? transaction = null);
    }
}
