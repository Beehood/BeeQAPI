using Models;
using System.Data;

namespace DAL.ContractIF
{
    public interface IDAL_BranchService
    {
        Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll( PaginationRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<BranchServiceModel>> GetById(long id,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(BranchServiceRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(BranchServiceRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>>
 GetBranchDropdownByOrganization(
     long orgId,
     string email,
     IDbTransaction? transaction = null
 );
    }
}