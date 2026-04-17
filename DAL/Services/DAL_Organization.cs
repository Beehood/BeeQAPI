using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class OrganizationDAL : IOrganizationDAL
    {
        private readonly DBConnection _config;

        public OrganizationDAL(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<OrganizationModel>>();

            try
            {
                int pageSize = request.PageSize;

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "LIST");
                param.Add("p_OrganizationId", null);
                param.Add("p_Name", null);
                param.Add("p_Email", null);
                param.Add("p_Phone", null);
                param.Add("p_Address", null);
                param.Add("p_SubscriptionPlan", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo );
                param.Add("p_PageSize", pageSize);
                param.Add("p_UserId", null);

                await conn.OpenAsync();

                using var multi = await conn.QueryMultipleAsync(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                int totalRecords = await multi.ReadFirstAsync<int>();
                response.TotalRecords = totalRecords;

                var list = (await multi.ReadAsync<OrganizationModel>()).ToList();

                if (list.Any())
                {
                    response.Result = list;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = new List<OrganizationModel>();
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<OrganizationModel>> GetById(long id, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<OrganizationModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "GETBYID");
                param.Add("p_OrganizationId", id);

                await conn.OpenAsync();

                var data = await conn.QueryFirstOrDefaultAsync<OrganizationModel>(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (data != null)
                {
                    response.Result = data;
                    response.IsSuccess = true;
                    response.TotalRecords = 1;
                }
                else
                {
                    response.Result = null;
                    response.IsSuccess = false;
                    response.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        public async Task<APIGetResponseModel<long>> Insert(OrganizationRequestDto request, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "INSERT");
                param.Add("p_OrganizationId", null);
                param.Add("p_Name", request.Name);
                param.Add("p_Email", request.Email);
                param.Add("p_Phone", request.Phone);
                param.Add("p_Address", request.Address);
                param.Add("p_SubscriptionPlan", request.SubscriptionPlan);
                param.Add("p_Status", 1);
                param.Add("p_UserId", request.UserId);

                await conn.OpenAsync();

                var id = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (id > 0)
                {
                    response.Result = id;
                    response.IsSuccess = true;
                    response.TotalRecords = 1;
                }
                else
                {
                    response.Result = 0;
                    response.IsSuccess = false;
                    response.TotalRecords = 0;
                    response.ErrorMsgs.Add("Insert failed");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<long>> Update(OrganizationRequestDto request, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "UPDATE");
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_Name", request.Name);
                param.Add("p_Email", request.Email);
                param.Add("p_Phone", request.Phone);
                param.Add("p_Address", request.Address);
                param.Add("p_SubscriptionPlan", request.SubscriptionPlan);
                param.Add("p_Status", request.Status);
                param.Add("p_UserId", request.UserId);

                await conn.OpenAsync();

                var id = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (id > 0)
                {
                    response.Result = id;
                    response.IsSuccess = true;
                    response.TotalRecords = 1;
                }
                else
                {
                    response.Result = 0;
                    response.IsSuccess = false;
                    response.TotalRecords = 0;
                    response.ErrorMsgs.Add("Update failed");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        public async Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<long>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "STATUS");
                param.Add("p_OrganizationId", id);
                param.Add("p_Status", status);
                param.Add("p_UserId", userId);

                await conn.OpenAsync();

                var result = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (result > 0)
                {
                    response.Result = result;
                    response.IsSuccess = true;
                    response.TotalRecords = 1;
                }
                else
                {
                    response.Result = 0;
                    response.IsSuccess = false;
                    response.TotalRecords = 0;
                    response.ErrorMsgs.Add("Status change failed");
                }
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
