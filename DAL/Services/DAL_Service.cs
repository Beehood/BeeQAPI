using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Helpers;
using Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace DAL.Implementation

{
    public class DAL_Service : IDAL_Service

    {
        private readonly DBConnection _config;
        public DAL_Service(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Service DAL Layer
        /// Author: Swapnalisa
        /// Description:
        /// Handles all database operations for Service module using stored procedure (sp_manage_service).

        public async Task<APIGetResponseModel<List<ServiceModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<ServiceModel>>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "LIST");
                param.Add("p_ServiceId", null);
                param.Add("p_ServiceName", null);
                param.Add("p_ServiceCode", null);
                param.Add("p_EstimatedTime", null);
                param.Add("p_Description", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_service", param, commandType: CommandType.StoredProcedure);
                response.TotalRecords = await multi.ReadFirstAsync<int>();
                var list = (await multi.ReadAsync<ServiceModel>()).ToList();
                response.Result = list;
                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching services");
                Console.WriteLine("DAL SERVICE GET ALL ERROR: " + ex.Message);
            }
            return response;
        }
        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Fetch paginated list of services
        /// Uses multi-result stored procedure (count + data)
        public async Task<APIGetResponseModel<ServiceModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {
            var response = new APIGetResponseModel<ServiceModel>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "GETBYID");
                param.Add("p_ServiceId", id);
                param.Add("p_ServiceName", null);
                param.Add("p_ServiceCode", null);
                param.Add("p_EstimatedTime", null);
                param.Add("p_Description", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<ServiceModel>("sp_manage_service", param, commandType: CommandType.StoredProcedure);

                if (data != null)
                {
                    response.Result = data;
                    response.TotalRecords = 1;
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching service");
                Console.WriteLine("DAL SERVICE GETBYID ERROR: " + ex.Message);
            }
            return response;
        }

        // ========================
        // INSERT
        // ========================
        /// <summary>
        /// Insert new service
        /// Returns newly created ServiceId
        public async Task<APIGetResponseModel<int>> Insert(ServiceRequestDto request, string email, IDbTransaction? transaction = null)

        {
            var response = new APIGetResponseModel<int>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "INSERT");
                param.Add("p_ServiceId", null);
                param.Add("p_ServiceName", request.ServiceName);
                param.Add("p_ServiceCode", request.ServiceCode);
                param.Add("p_EstimatedTime", request.EstimatedTime);
                param.Add("p_Description", request.Description);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_service", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while inserting service");
                Console.WriteLine("DAL SERVICE INSERT ERROR: " + ex.Message);
            }
            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// Update existing service
        /// Returns updated ServiceId

        public async Task<APIGetResponseModel<int>> Update(ServiceRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");
                param.Add("p_ServiceId", request.ServiceId);
                param.Add("p_ServiceName", request.ServiceName);
                param.Add("p_ServiceCode", request.ServiceCode);
                param.Add("p_EstimatedTime", request.EstimatedTime);
                param.Add("p_Description", request.Description);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_service", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)

            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating service");

                Console.WriteLine("DAL SERVICE UPDATE ERROR: " + ex.Message);
            }
            return response;
        }
        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Activate / Deactivate service

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");
                param.Add("p_ServiceId", id);
                param.Add("p_ServiceName", null);
                param.Add("p_ServiceCode", null);
                param.Add("p_EstimatedTime", null);
                param.Add("p_Description", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_service", param, commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = result > 0;
                response.TotalRecords = result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while changing service status");
                Console.WriteLine("DAL SERVICE STATUS ERROR: " + ex.Message);
            }
            return response;
        }
        // ========================
        // DROPDOWN
        // ========================
        /// <summary>
        /// Fetch active services for dropdown

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();
            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");
                param.Add("p_ServiceId", null);
                param.Add("p_ServiceName", null);
                param.Add("p_ServiceCode", null);
                param.Add("p_EstimatedTime", null);
                param.Add("p_Description", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_service", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = data.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching service dropdown");

                Console.WriteLine("DAL SERVICE DROPDOWN ERROR: " + ex.Message);
            }
            return response;
        }
    }
}
