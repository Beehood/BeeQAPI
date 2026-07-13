using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Branch
    {
        Task<APIGetResponseModel<List<BranchModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<BranchModel>> GetById(long id, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(BranchRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(BranchRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id,string email,IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,long? organizationId,IDbTransaction? transaction = null);
    }
}
