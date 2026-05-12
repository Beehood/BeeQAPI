using Models;
using System.Data;

namespace BAL.ContractIF
{
    public interface IBAL_BranchService

    {
        Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,  IDbTransaction? transaction = null);

        Task<APIGetResponseModel<BranchServiceModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null );

        Task<APIGetResponseModel<int>> Create(BranchServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(BranchServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetBranchDropdownByOrganization(long orgId,string email,IDbTransaction? transaction = null);
       
    }
}
