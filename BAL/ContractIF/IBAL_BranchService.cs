using Models;
using System.Data;

namespace BAL.ContractIF.BAL.ContractIF
{
    public interface IBAL_BranchService
    {
        Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<BranchServiceModel>> GetById(long id, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Create(BranchServiceRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(BranchServiceRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(TokenUserInfo user, IDbTransaction? transaction = null);
    }
}