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
        Task<APIGetResponseModel<List<CounterModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<CounterModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(CounterRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(CounterRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null);
    }
}

