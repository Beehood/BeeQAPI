using DAL.ContractIF;
using DAL.ContractIF.DAL.ContractIF;
using Dapper;
using Helpers;
using Models;
using System.Data;

namespace DAL.Implementation
{
    public class DAL_Service : IDAL_Service
    {
        private readonly IDbConnection _db;

        public DAL_Service(IDbConnection db)
        {
            _db = db;
        }

        public async Task<APIGetResponseModel<List<ServiceModel>>> ServiceList(ServiceSearchKeys obj, IDbTransaction? transaction)
        {
            var response = new APIGetResponseModel<List<ServiceModel>>();

            try
            {
                var param = ServiceParamHelper.GetBaseParams();

                param.Add("p_Action", "LIST");
                param.Add("p_SearchKey", obj.SearchKey ?? "");
                param.Add("p_PageNo", obj.PageNo);
                param.Add("p_PageSize", obj.PageSize);

                using var multi = await _db.QueryMultipleAsync(
                    "sp_services",
                    param,
                    transaction,
                    commandType: CommandType.StoredProcedure);

                var total = await multi.ReadFirstAsync<int>();
                var data = (await multi.ReadAsync<ServiceModel>()).ToList();

                response.Result = data;
                response.TotalRecords = total;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        public async Task<APIGetResponseModel<ServiceModel>> ServiceById(ServiceSearchKeys obj, IDbTransaction? transaction)
        {
            var response = new APIGetResponseModel<ServiceModel>();

            try
            {
                var param = ServiceParamHelper.GetBaseParams();

                param.Add("p_Action", "GETBYID");
                param.Add("p_service_id",obj.Id);

                var data = await _db.QueryFirstOrDefaultAsync<ServiceModel>(
                    "sp_services",
                    param,
                    transaction,
                    commandType: CommandType.StoredProcedure);

                response.Result = data;
                response.IsSuccess = data != null;

                if (data == null)
                    response.ErrorMsgs.Add("Service not found");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        public async Task<APIGetResponseModel<int>> ServiceCreate(ServiceModel data, string userId, IDbTransaction? transaction)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                var param = ServiceParamHelper.GetBaseParams();

                param.Add("p_Action", "INSERT");
                param.Add("p_service_name", data.ServiceName);
                param.Add("p_service_code", data.ServiceCode);
                param.Add("p_estimated_time", data.EstimatedTime);
                param.Add("p_description", data.Description);
                param.Add("p_user_id", userId);

                var result = await _db.ExecuteScalarAsync<int>(
                    "sp_services",
                    param,
                    transaction,
                    commandType: CommandType.StoredProcedure);

                // 🔥 Handle duplicate
                if (result == -1)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Service code already exists");
                }
                else
                {
                    response.Result = result;
                    response.IsSuccess = result > 0;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        public async Task<APIGetResponseModel<int>> ServiceUpdate(ServiceModel data, string userId, IDbTransaction? transaction)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                var param = ServiceParamHelper.GetBaseParams();

                param.Add("p_Action", "UPDATE");
                param.Add("p_service_id", data.Service_id);
                param.Add("p_service_name", data.ServiceName);
                param.Add("p_service_code", data.ServiceCode);
                param.Add("p_estimated_time", data.EstimatedTime);
                param.Add("p_description", data.Description);
                param.Add("p_user_id", userId);

                var result = await _db.ExecuteScalarAsync<int>(
                    "sp_services",
                    param,
                    transaction,
                    commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = result > 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        public async Task<APIGetResponseModel<int>> ServiceStatus(ServiceSearchKeys obj, string userId, IDbTransaction? transaction)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                var param = ServiceParamHelper.GetBaseParams();

                param.Add("p_Action", "STATUS");
                param.Add("p_service_id", obj.Id);
                param.Add("p_user_id", userId);

                var result = await _db.ExecuteScalarAsync<int>(
                    "sp_services",
                    param,
                    transaction,
                    commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = result > 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }
    }
}