using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    using Models;
    using System.Data;

    namespace DAL.ContractIF
    {
        public interface IDAL_Service
        {
            Task<APIGetResponseModel<List<ServiceModel>>> ServiceList(ServiceSearchKeys obj, IDbTransaction? transaction);
            Task<APIGetResponseModel<ServiceModel>> ServiceById(ServiceSearchKeys obj, IDbTransaction? transaction);
            Task<APIGetResponseModel<int>> ServiceCreate(ServiceModel data, string userId, IDbTransaction? transaction);
            Task<APIGetResponseModel<int>> ServiceUpdate(ServiceModel data, string userId, IDbTransaction? transaction);
            Task<APIGetResponseModel<int>> ServiceStatus(ServiceSearchKeys obj, string userId, IDbTransaction? transaction);
        }
    }
}
