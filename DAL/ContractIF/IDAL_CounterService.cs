using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_CounterService
    {
        Task<APIGetResponseModel<List<CounterServiceModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<CounterServiceModel>> GetById(long id, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Insert(CounterServiceRequestDto request, string userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(CounterServiceRequestDto request, string userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null);
    }
}
