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
    public class DAL_Organization : IDAL_Organization
    {
        private readonly DBConnection _config;

        public DAL_Organization(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL (Dynamic - Multi Result)
        // ========================
        public async Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(
            PaginationRequestDto request,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<OrganizationModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "LIST");
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_PageSize", request.PageSize);

                using var multi = await conn.QueryMultipleAsync(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                // 🔹 1st Result → Total Count
                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // 🔹 2nd Result → Data
                var list = (await multi.ReadAsync<OrganizationModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching organizations");
                Console.WriteLine("DAL GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<OrganizationModel>> GetById(
            long id,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<OrganizationModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "GETBYID");
                param.Add("p_OrganizationId", id);

                var data = await conn.QueryFirstOrDefaultAsync<OrganizationModel>(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (data != null)
                {
                    response.Result = data;
                    response.TotalRecords = 1;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = null;
                    response.TotalRecords = 0;
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching organization");
                Console.WriteLine("DAL GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        public async Task<APIGetResponseModel<long>> Insert(
            OrganizationRequestDto request,
            string userId,
            IDbTransaction? transaction = null)
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
                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while inserting organization");
                Console.WriteLine("DAL INSERT ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<long>> Update(
            OrganizationRequestDto request,
            string userId,
            IDbTransaction? transaction = null)
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
                param.Add("p_UserId", userId);

                var id = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                response.Result = id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating organization");
                Console.WriteLine("DAL UPDATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        public async Task<APIGetResponseModel<long>> ChangeStatus(
            long id,
            int status,
            long userId,
            IDbTransaction? transaction = null)
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

                var result = await conn.ExecuteScalarAsync<long>(
                    "sp_manage_organization",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                response.Result = result;
                response.IsSuccess = result > 0;
                response.TotalRecords = result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while changing status");
                Console.WriteLine("DAL STATUS ERROR: " + ex.Message);
            }

            return response;
        }
    }
}
