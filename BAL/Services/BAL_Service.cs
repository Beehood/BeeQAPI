using BAL.ContractIF;
using BAL.ContractIF.BAL.ContractIF;
using DAL.ContractIF.DAL.ContractIF;
using Models;
using System.Data;

namespace BAL.Implementation
{
    public class BAL_Service : IBAL_Service
    {
        private readonly IDAL_Service _service;

        public BAL_Service(IDAL_Service service)
        {
            _service = service;
        }

        // 🔥 LIST
        public async Task<APIGetResponseModel<List<ServiceModel>>> ServiceList(ServiceSearchKeys obj, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<List<ServiceModel>>();

            if (obj == null)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid request data");
                return res;
            }

            return await _service.ServiceList(obj, transaction);
        }

        // 🔥 GET BY ID
        public async Task<APIGetResponseModel<ServiceModel>> ServiceById(ServiceSearchKeys obj, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<ServiceModel>();

            if (obj == null || obj.Id <= 0)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid Service Id");
                return res;
            }

            return await _service.ServiceById(obj, transaction);
        }

        // 🔥 CREATE
        public async Task<APIGetResponseModel<int>> ServiceCreate(ServiceModel data, string userId, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<int>();

            if (data == null)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid data");
                return res;
            }

            if (string.IsNullOrWhiteSpace(data.ServiceName))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Service Name is required");
                return res;
            }

            if (string.IsNullOrEmpty(userId))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Unauthorized user");
                return res;
            }

            return await _service.ServiceCreate(data, userId, transaction);
        }

        // 🔥 UPDATE
        public async Task<APIGetResponseModel<int>> ServiceUpdate(ServiceModel data, string userId, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<int>();

            if (data == null || data.Service_id <= 0)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid service data");
                return res;
            }

            if (string.IsNullOrWhiteSpace(data.ServiceName))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Service Name is required");
                return res;
            }

            if (string.IsNullOrEmpty(userId))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Unauthorized user");
                return res;
            }

            return await _service.ServiceUpdate(data, userId, transaction);
        }

        // 🔥 STATUS
        public async Task<APIGetResponseModel<int>> ServiceStatus(ServiceSearchKeys obj, string userId, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<int>();

            if (obj == null || obj.Id <= 0)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid Service Id");
                return res;
            }

            if (string.IsNullOrEmpty(userId))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Unauthorized user");
                return res;
            }

            return await _service.ServiceStatus(obj, userId, transaction);
        }

        // 🔥 DROPDOWN (SaaS READY)
        //public async Task<APIGetResponseModel<List<ModelDropdown>>> ServiceDropdown(int orgId, IDbTransaction? transaction)
        //{
        //    var res = new APIGetResponseModel<List<ModelDropdown>>();

        //    if (orgId <= 0)
        //    {
        //        res.IsSuccess = false;
        //        res.ErrorMsgs.Add("Invalid Organization");
        //        return res;
        //    }

        //    return await _service.ServiceDropdown(orgId, transaction);
        //}
    }
}