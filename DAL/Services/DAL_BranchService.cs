using DAL.ContractIF.DAL.ContractIF;
using Dapper;
using Models;
using System.Data;

namespace DAL.Implementation
{
    public class DAL_BranchService : IDAL_BranchService
    {
        private readonly IDbConnection _db;

        public DAL_BranchService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<APIGetResponseModel<List<BranchServiceModel>>> BranchServiceList(BranchServiceSearchKeys obj, IDbTransaction? transaction)
        {
            var res = new APIGetResponseModel<List<BranchServiceModel>>();

            var param = new DynamicParameters();
            param.Add("p_Action", "LIST");
            param.Add("p_branch_service_id", 0);
            param.Add("p_branch_id", 0);
            param.Add("p_service_id", 0);
            param.Add("p_prefix", "");
            param.Add("p_daily_limit", 0);
            param.Add("p_organization_id", 1); // 🔥 TEMP (later from JWT)
            param.Add("p_SearchKey", obj.SearchKey ?? "");
            param.Add("p_PageNo", obj.PageNo);
            param.Add("p_PageSize", obj.PageSize);
            param.Add("p_user_id", "");

            using var multi = await _db.QueryMultipleAsync("sp_branch_services", param, transaction, commandType: CommandType.StoredProcedure);

            res.TotalRecords = await multi.ReadFirstAsync<int>();
            res.Result = (await multi.ReadAsync<BranchServiceModel>()).ToList();
            res.IsSuccess = true;

            return res;
        }

        public async Task<APIGetResponseModel<BranchServiceModel>> BranchServiceById(BranchServiceSearchKeys obj, IDbTransaction? transaction)
        {
            var param = new DynamicParameters();

            param.Add("p_Action", "GETBYID");
            param.Add("p_branch_service_id", obj.branch_service_id); // ✅ FIXED
            param.Add("p_organization_id", 1); // (later from JWT)

            var data = await _db.QueryFirstOrDefaultAsync<BranchServiceModel>(
                "sp_branch_services",
                param,
                transaction,
                commandType: CommandType.StoredProcedure);

            return new APIGetResponseModel<BranchServiceModel>
            {
                Result = data,
                IsSuccess = data != null
            };
        }

        public async Task<APIGetResponseModel<int>> BranchServiceCreate(BranchServiceModel data, string userId, IDbTransaction? transaction)
        {
            var param = new DynamicParameters();
            param.Add("p_Action", "INSERT");
            param.Add("p_branch_id", data.branch_id);
            param.Add("p_service_id", data.service_id);
            param.Add("p_prefix", data.prefix);
            param.Add("p_daily_limit", data.daily_limit);
            param.Add("p_organization_id", 1);
            param.Add("p_user_id", userId);

            var result = await _db.ExecuteScalarAsync<int>(
                "sp_branch_services", param, transaction, commandType: CommandType.StoredProcedure);

            return new APIGetResponseModel<int> { Result = result, IsSuccess = result > 0 };
        }

        public async Task<APIGetResponseModel<int>> BranchServiceUpdate(BranchServiceModel data, string userId, IDbTransaction? transaction)
        {
            var param = new DynamicParameters();
            param.Add("p_Action", "UPDATE");
            param.Add("p_branch_service_id", data.branch_service_id);
            param.Add("p_branch_id", data.branch_id);
            param.Add("p_service_id", data.service_id);
            param.Add("p_prefix", data.prefix);
            param.Add("p_daily_limit", data.daily_limit);
            param.Add("p_organization_id", 1);
            param.Add("p_user_id", userId);

            var result = await _db.ExecuteScalarAsync<int>(
                "sp_branch_services", param, transaction, commandType: CommandType.StoredProcedure);

            return new APIGetResponseModel<int> { Result = result, IsSuccess = result > 0 };
        }

        public async Task<APIGetResponseModel<int>> BranchServiceStatus(BranchServiceSearchKeys obj, string userId, IDbTransaction? transaction)
        {
            var param = new DynamicParameters();
            param.Add("p_Action", "STATUS");
            param.Add("p_branch_service_id", obj.branch_service_id);
            param.Add("p_organization_id", 1);
            param.Add("p_user_id", userId);

            var result = await _db.ExecuteScalarAsync<int>(
                "sp_branch_services", param, transaction, commandType: CommandType.StoredProcedure);

            return new APIGetResponseModel<int> { Result = result, IsSuccess = result > 0 };
        }
       
    }
}