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
        Task<APIGetResponseModel<List<BranchModel>>> GetAll(PaginationRequestDto request,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<BranchModel>> GetById(long id,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Insert(BranchRequestDto request,string userId,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(BranchRequestDto request,string userId,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(long id,int status,long userId,IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null);
    }
}
