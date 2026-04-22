using BAL.ContractIF.BAL.ContractIF;
using DAL.ContractIF.DAL.ContractIF;
using Models;
using System.Data;

namespace BAL.Implementation
{
    public class BAL_BranchService : IBAL_BranchService
    {
        private readonly IDAL_BranchService _dal;

        public BAL_BranchService(IDAL_BranchService dal)
        {
            _dal = dal;
        }

        // 🔥 LIST
        public async Task<APIGetResponseModel<List<BranchServiceModel>>> BranchServiceList(BranchServiceSearchKeys obj, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<List<BranchServiceModel>>();

            if (obj == null)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid request data");
                return res;
            }

            return await _dal.BranchServiceList(obj, transaction);
        }

        // 🔥 GET BY ID (FIXED)
        public async Task<APIGetResponseModel<BranchServiceModel>> BranchServiceById(BranchServiceSearchKeys obj, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<BranchServiceModel>();

            if (obj == null || obj.branch_service_id <= 0)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid Branch Service Id");
                return res;
            }

            return await _dal.BranchServiceById(obj, transaction); // ✅ pass object
        }

        // 🔥 CREATE
        public async Task<APIGetResponseModel<int>> BranchServiceCreate(BranchServiceModel data, string userId, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<int>();

            if (data == null || data.branch_id <= 0 || data.service_id <= 0)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid branch/service data");
                return res;
            }

            if (string.IsNullOrWhiteSpace(data.prefix))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Prefix is required");
                return res;
            }

            if (string.IsNullOrEmpty(userId))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Unauthorized user");
                return res;
            }

            return await _dal.BranchServiceCreate(data, userId, transaction);
        }

        // 🔥 UPDATE
        public async Task<APIGetResponseModel<int>> BranchServiceUpdate(BranchServiceModel data, string userId, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<int>();

            if (data == null || data.branch_service_id <= 0)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid Branch Service Id");
                return res;
            }

            if (data.branch_id <= 0 || data.service_id <= 0)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid branch/service");
                return res;
            }

            if (string.IsNullOrWhiteSpace(data.prefix))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Prefix is required");
                return res;
            }

            if (string.IsNullOrEmpty(userId))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Unauthorized user");
                return res;
            }

            return await _dal.BranchServiceUpdate(data, userId, transaction);
        }

        // 🔥 STATUS
        public async Task<APIGetResponseModel<int>> BranchServiceStatus(BranchServiceSearchKeys obj, string userId, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<int>();

            if (obj == null || obj.branch_service_id <= 0)
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Invalid Branch Service Id");
                return res;
            }

            if (string.IsNullOrEmpty(userId))
            {
                res.IsSuccess = false;
                res.ErrorMsgs.Add("Unauthorized user");
                return res;
            }

            return await _dal.BranchServiceStatus(obj, userId, transaction);
        }
    }
}