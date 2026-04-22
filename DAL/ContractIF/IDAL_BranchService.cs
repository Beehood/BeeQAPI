using Models;
using System.Data;

namespace DAL.ContractIF.DAL.ContractIF
{
    public interface IDAL_BranchService
    {
        Task<APIGetResponseModel<List<BranchServiceModel>>> BranchServiceList(BranchServiceSearchKeys obj, IDbTransaction? transaction);

        Task<APIGetResponseModel<BranchServiceModel>> BranchServiceById(BranchServiceSearchKeys obj, IDbTransaction? transaction);

        Task<APIGetResponseModel<int>> BranchServiceCreate(BranchServiceModel data, string userId, IDbTransaction? transaction);

        Task<APIGetResponseModel<int>> BranchServiceUpdate(BranchServiceModel data, string userId, IDbTransaction? transaction);

        Task<APIGetResponseModel<int>> BranchServiceStatus(BranchServiceSearchKeys obj, string userId, IDbTransaction? transaction);
        
    }
}