using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_Branch
    {
        Task<APIGetResponseModel<List<BranchModel>>> GetAll(PaginationRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<BranchModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(BranchRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(BranchRequestDto request,List<string> roles, string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email,IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,long? organizationId,IDbTransaction? transaction = null);
    }
}
