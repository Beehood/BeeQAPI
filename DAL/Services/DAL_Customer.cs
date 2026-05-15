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
    public class DAL_Customer : IDAL_Customer
    {
        private readonly DBConnection _config;

        public DAL_Customer(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Customer DAL - Get All Customers
        /// Description:- Fetches paginated customer list using stored procedure (multi-result).
        /// </summary>
        /// <param name="request">PaginationRequestDto</param>
        /// <param name="email">Logged in User Email</param>
        /// <param name="transaction">Optional DB transaction</param>
        /// <returns>Returns list of CustomerModel with total count</returns>
        public async Task<APIGetResponseModel<List<CustomerModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<CustomerModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");
                param.Add("p_CustomerId", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_Name", null);
                param.Add("p_Email", null);
                param.Add("p_Phone", null);
                param.Add("p_IsVip", null);
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_UserEmail", email); //  IMPORTANT (RBAC handled in SP)
                using var multi = await conn.QueryMultipleAsync("sp_manage_customer", param, commandType: CommandType.StoredProcedure);
                response.TotalRecords = await multi.ReadFirstAsync<int>();
                var list = (await multi.ReadAsync<CustomerModel>()).ToList();
                response.Result = list;
                response.IsSuccess = true; 
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching customers");
                Console.WriteLine("DAL CUSTOMER GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Customer DAL - Get Customer By Id
        /// Description:- Fetches single customer using CustomerId.
        /// </summary>
        public async Task<APIGetResponseModel<CustomerModel>> GetById(long id, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<CustomerModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "GETBYID");
                param.Add("p_CustomerId", id);
                param.Add("p_OrganizationId", null);
                param.Add("p_Name", null);
                param.Add("p_Email", null);
                param.Add("p_Phone", null);
                param.Add("p_IsVip", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);
                var data = await conn.QueryFirstOrDefaultAsync<CustomerModel>("sp_manage_customer", param, commandType: CommandType.StoredProcedure);
                response.Result = data;
                response.IsSuccess = data != null;
                response.TotalRecords = data != null ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching customer");
                Console.WriteLine("DAL CUSTOMER GETBYID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        /// <summary>
        /// Customer DAL - Insert Customer
        /// Description:- Inserts new customer record using stored procedure.
        /// </summary>
        public async Task<APIGetResponseModel<int>> Insert(CustomerRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "INSERT");
                param.Add("p_CustomerId", null);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_Name", request.Name);
                param.Add("p_Email", request.Email);
                param.Add("p_Phone", request.Phone);
                param.Add("p_IsVip", request.IsVip);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);
                var id = await conn.ExecuteScalarAsync<long>("sp_manage_customer", param, commandType: CommandType.StoredProcedure);
                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while inserting customer");
                Console.WriteLine("DAL CUSTOMER INSERT ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        /// <summary>
        /// Customer DAL - Update Customer
        /// Description:- Updates existing customer details.
        /// </summary>
        public async Task<APIGetResponseModel<int>> Update(CustomerRequestDto request, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "UPDATE");
                param.Add("p_CustomerId", request.CustomerId);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_Name", request.Name);
                param.Add("p_Email", request.Email);
                param.Add("p_Phone", request.Phone);
                param.Add("p_IsVip", request.IsVip);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);
                var id = await conn.ExecuteScalarAsync<long>("sp_manage_customer", param, commandType: CommandType.StoredProcedure);
                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating customer");
                Console.WriteLine("DAL CUSTOMER UPDATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // STATUS
        // ========================
        /// <summary>
        /// Customer DAL - Change Customer Status
        /// Description:- Updates customer status (Active/Inactive).
        /// </summary>
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "STATUS");
                param.Add("p_CustomerId", id);
                param.Add("p_OrganizationId", null);
                param.Add("p_Name", null);
                param.Add("p_Email", null);
                param.Add("p_Phone", null);
                param.Add("p_IsVip", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);
                var result = await conn.ExecuteScalarAsync<int>("sp_manage_customer", param, commandType: CommandType.StoredProcedure);
                response.Result = result;
                response.IsSuccess = result > 0;
                response.TotalRecords = result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while changing customer status");
                Console.WriteLine("DAL CUSTOMER STATUS ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        /// <summary>
        /// Customer DAL - Get Customer Dropdown
        /// Description:- Fetches active customer dropdown list.
        /// </summary>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);
                var param = new DynamicParameters();
                param.Add("p_Action", "DROPDOWN");
                param.Add("p_CustomerId", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_Name", null);
                param.Add("p_Email", null);
                param.Add("p_Phone", null);
                param.Add("p_IsVip", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);
                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_customer", param, commandType: CommandType.StoredProcedure)).ToList();
                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching customer dropdown");
                Console.WriteLine("DAL CUSTOMER DROPDOWN ERROR: " + ex.Message);
            }

            return response;
        }
    }
}

