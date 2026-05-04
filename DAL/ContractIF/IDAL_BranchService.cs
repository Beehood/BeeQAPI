using Models;
using System.Data;

namespace DAL.ContractIF.DAL.ContractIF
{
    public interface IDAL_BranchService
    {
        Task<APIGetResponseModel<List<BranchServiceModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<BranchServiceModel>> GetById(long id, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Insert(BranchServiceRequestDto request, string userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(BranchServiceRequestDto request, string userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null);
    }
}