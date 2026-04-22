using Models;
using System.Data;

namespace BAL.ContractIF
{
  
    namespace BAL.ContractIF
    {
        public interface IBAL_Service
        {
            Task<APIGetResponseModel<List<ServiceModel>>> ServiceList(ServiceSearchKeys obj, IDbTransaction? transaction);
            Task<APIGetResponseModel<ServiceModel>> ServiceById(ServiceSearchKeys obj, IDbTransaction? transaction);
            Task<APIGetResponseModel<int>> ServiceCreate(ServiceModel data, string userId, IDbTransaction? transaction);
            Task<APIGetResponseModel<int>> ServiceUpdate(ServiceModel data, string userId, IDbTransaction? transaction);
            Task<APIGetResponseModel<int>> ServiceStatus(ServiceSearchKeys obj, string userId, IDbTransaction? transaction);
        }
    }
}
