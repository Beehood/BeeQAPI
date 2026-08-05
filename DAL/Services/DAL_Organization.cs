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

        /// <summary>

        /// Organization DAL - Get All Organizations

        /// Author: Swapnlisa

        /// Description:- Fetches paginated organization list using stored procedure (multi-result).

        /// <param name="request">PaginationRequestDto</param>

        /// <param name="transaction">Optional DB transaction</param>

        /// <returns>Returns list of OrganizationModel with total count</returns>

        public async Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<OrganizationModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_OrganizationId", null);

                param.Add("p_Name", null);

                param.Add("p_Email", null);

                param.Add("p_Phone", null);

                param.Add("p_Address", null);

                param.Add("p_SubscriptionPlan", null);

                //param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                //param.Add("p_PageSize", request.PageSize);

                param.Add("p_UserEmail", null);

                using var multi = await conn.QueryMultipleAsync("sp_manage_organization", param, commandType: CommandType.StoredProcedure);

                //  1st Result → Total Count

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                // 2nd Result → Data

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

        /// <summary>

        /// Organization DAL - Get Organization By Id

        /// Author: Swapnlisa

        /// Description:- Fetches single organization using OrganizationId.

        /// </summary>

        /// <param name="id">OrganizationId</param>

        /// <param name="transaction">Optional DB transaction</param>

        /// <returns>Returns OrganizationModel</returns>

        public async Task<APIGetResponseModel<OrganizationModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<OrganizationModel>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_OrganizationId", id);

                param.Add("p_Name", null);

                param.Add("p_Email", null);

                param.Add("p_Phone", null);

                param.Add("p_Address", null);

                param.Add("p_SubscriptionPlan", null);

                //param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                //param.Add("p_PageSize", null);

                param.Add("p_UserEmail", null);

                var data = await conn.QueryFirstOrDefaultAsync<OrganizationModel>("sp_manage_organization", param, commandType: CommandType.StoredProcedure);

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

        /// <summary>

        /// Organization DAL - Insert Organization

        /// Author: Swapnlisa

        /// Description:- Inserts new organization record using stored procedure.

        /// <param name="request">OrganizationRequestDto</param>

        /// <param name="userId">Logged in UserId</param>

        /// <param name="transaction">Optional DB transaction</param>

        /// <returns>Returns newly created OrganizationId</returns>

        public async Task<APIGetResponseModel<int>> Insert(OrganizationRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

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

                //param.Add("p_Status", 1);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                //param.Add("p_PageSize", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_organization", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

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

        /// <summary>

        /// Organization DAL - Update Organization

        /// Author: Swapnlisa

        /// Description:- Updates existing organization details.

        /// <param name="request">OrganizationRequestDto</param>

        /// <param name="userId">Logged in UserId</param>

        /// <param name="transaction">Optional DB transaction</param>

        /// <returns>Returns updated OrganizationId</returns>

        public async Task<APIGetResponseModel<int>> Update(OrganizationRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

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

                //param.Add("p_Status", request.Status);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                //param.Add("p_PageSize", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_organization", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

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

        /// <summary>

        /// Organization DAL - Change Status

        /// Author: Swapnlisa

        /// Description:- Updates organization status (Active/Inactive).

        /// <param name="id">OrganizationId</param>

        /// <param name="status">0 = Inactive, 1 = Active</param>

        /// <param name="userId">Logged in UserId</param>

        /// <param name="transaction">Optional DB transaction</param>

        /// <returns>Returns status update result</returns>

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                // ✅ 1. START DEBUG (TOP)

                Console.WriteLine("=== DAL START ===");

                Console.WriteLine($"OrgId: {id}");

                Console.WriteLine($"Email: {email}");

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_OrganizationId", id);

                param.Add("p_Name", null);

                param.Add("p_Email", null);

                param.Add("p_Phone", null);

                param.Add("p_Address", null);

                param.Add("p_SubscriptionPlan", null);

                //param.Add("p_Status", status);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", 1);

                //param.Add("p_PageSize", null);

                param.Add("p_UserEmail", email);

                // ✅ 2. BEFORE SP CALL

                Console.WriteLine("Calling SP with params:");

                Console.WriteLine("p_Action = STATUS");

                Console.WriteLine("p_OrganizationId = " + id);

                Console.WriteLine("p_UserEmail = " + email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_organization", param, commandType: CommandType.StoredProcedure);

                // ✅ 3. AFTER SP CALL

                Console.WriteLine("SP RESULT: " + result);

                response.Result = result;

                response.IsSuccess = true;

                response.TotalRecords = 1;

            }

            catch (Exception ex)

            {

                // ✅ ERROR DEBUG

                Console.WriteLine("❌ DAL ERROR:");

                Console.WriteLine(ex.Message);

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while changing status");

                Console.WriteLine("DAL STATUS ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // DROPDOWN

        // ========================

        /// <summary>

        /// Organization DAL - Get Organization Dropdown

        /// Author: Swapnlisa

        /// Description:- Fetches active organization dropdown list.

        /// </summary>

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<DropdownModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");

                param.Add("p_OrganizationId", null);

                param.Add("p_Name", null);

                param.Add("p_Email", null);

                param.Add("p_Phone", null);

                param.Add("p_Address", null);

                param.Add("p_SubscriptionPlan", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_organization", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = data.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching organization dropdown");

                Console.WriteLine("DAL ORGANIZATION DROPDOWN ERROR: " + ex.Message);

            }

            return response;

        }

    }

}

