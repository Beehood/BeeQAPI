using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Customer
    {
        Task<APIGetResponseModel<List<CustomerModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<CustomerModel>> GetById(long id, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<int>> Insert(CustomerRequestDto request, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<int>> Update(CustomerRequestDto request, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null);
    }
}
