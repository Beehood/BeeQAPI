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
        Task<APIGetResponseModel<List<CounterModel>>> GetAll(PaginationRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<CounterModel>> GetById(long id,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(CounterRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(CounterRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null);
    }
}
