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
    public class DAL_Token : IDAL_Token
    {
        private readonly DBConnection _config;

        public DAL_Token(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL (LIST)
        // ========================
        /// <summary>
        /// Token DAL - Get All Tokens
        /// Author: Swapnlisa
        /// Description:- Fetches paginated token list using stored procedure (multi-result).
        /// </summary>
        public async Task<APIGetResponseModel<List<TokenModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<TokenModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");
                param.Add("p_token_id", null);
                param.Add("p_counter_id", null);
                param.Add("p_user_id", null);
                param.Add("p_branch_service_id", null);

                param.Add("p_organization_id", null);   
                param.Add("p_branch_id", null);        
                param.Add("p_status", null);
                param.Add("p_token_date", null);        

                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);

                using var multi = await conn.QueryMultipleAsync("sp_manage_token", param, commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<TokenModel>()).ToList();

                response.Result = list;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching tokens");
                Console.WriteLine("DAL TOKEN GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Token DAL - Get Token By Id
        /// </summary>
        public async Task<APIGetResponseModel<TokenModel>> GetById(long id, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<TokenModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");
                param.Add("p_token_id", id);
                param.Add("p_counter_id", null);
                param.Add("p_user_id", null);
                param.Add("p_branch_service_id", null);

                // 🔥 ADD THESE (MISSING)
                param.Add("p_organization_id", null);
                param.Add("p_branch_id", null);
                param.Add("p_status", null);
                param.Add("p_token_date", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);

                var data = await conn.QueryFirstOrDefaultAsync<TokenModel>("sp_manage_token", param, commandType: CommandType.StoredProcedure);

                response.Result = data;
                response.IsSuccess = data != null;
                response.TotalRecords = data != null ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching token");
                Console.WriteLine("DAL TOKEN GETBYID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GENERATE TOKEN
        // ========================
        /// <summary>
        /// Token DAL - Generate Token
        /// Description:- Calls sp_generate_token
        /// </summary>
        public async Task<APIGetResponseModel<int>> GenerateToken(TokenRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_org_id", request.OrganizationId);
                param.Add("p_branch_id", request.BranchId);
                param.Add("p_branch_service_id", request.BranchServiceId);
                param.Add("p_customer_name", request.CustomerName);
                param.Add("p_customer_phone", request.CustomerPhone);

                var result = await conn.QueryFirstAsync<dynamic>("sp_generate_token", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)result.token_id;
                response.IsSuccess = response.Result > 0;
                response.TotalRecords = response.Result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while generating token");
                Console.WriteLine("DAL TOKEN GENERATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        /// <summary>
        /// Token DAL - Change Token Status
        /// </summary>
        public async Task<APIGetResponseModel<int>> ChangeStatus(TokenRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");
                param.Add("p_token_id", request.TokenId);
                param.Add("p_counter_id", request.CounterId);
                param.Add("p_user_id", null);
                param.Add("p_branch_service_id", null);
                param.Add("p_organization_id", null);
                param.Add("p_branch_id", null);
            
                param.Add("p_status", request.Status);
                param.Add("p_token_date", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_token", param, commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = result > 0;
                response.TotalRecords = result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating token status");
                Console.WriteLine("DAL TOKEN STATUS ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // CALL NEXT TOKEN
        // ========================
        /// <summary>
        /// Token DAL - Call Next Token
        /// </summary>
        public async Task<APIGetResponseModel<TokenModel>> CallNextToken(TokenRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<TokenModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "CALLNEXT");
                param.Add("p_token_id", null);
                param.Add("p_counter_id", request.CounterId);
                param.Add("p_user_id", null);
                param.Add("p_branch_service_id", request.BranchServiceId);
                param.Add("p_status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);

                var data = await conn.QueryFirstOrDefaultAsync<TokenModel>("sp_manage_token", param, commandType: CommandType.StoredProcedure);

                response.Result = data;
                response.IsSuccess = data != null;
                response.TotalRecords = data != null ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while calling next token");
                Console.WriteLine("DAL TOKEN CALLNEXT ERROR: " + ex.Message);
            }

            return response;
        }
        public async Task
<APIGetResponseModel<List<TokenStatusModel>>>
GetStatuses(
    string email,
    IDbTransaction? transaction = null)
        {
            var response =
                new APIGetResponseModel
                <List<TokenStatusModel>>();

            try
            {
                using var conn =
                    new MySqlConnection(
                        _config.DefaultConnection
                    );

                var list =
                    (await conn.QueryAsync<TokenStatusModel>(
                        @"SELECT
                    status_id AS StatusId,
                    status_name AS StatusName
                  FROM token_status_master
                  ORDER BY status_name"
                    )).ToList();

                response.Result = list;

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        /// <summary>
        /// Token DAL - Get Token Dropdown
        /// </summary>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");
                param.Add("p_token_id", null);
                param.Add("p_counter_id", null);
                param.Add("p_user_id", null);
                param.Add("p_branch_service_id", null);
                param.Add("p_status", null);

                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_token", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching token dropdown");
                Console.WriteLine("DAL TOKEN DROPDOWN ERROR: " + ex.Message);
            }

            return response;
        }
    }

}
