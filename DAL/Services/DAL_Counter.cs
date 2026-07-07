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

    public class DAL_Counter : IDAL_Counter

    {

        private readonly DBConnection _config;

        public DAL_Counter(DBConnection config)

        {

            _config = config;

        }

        // ========================

        // GET ALL

        // ========================

        /// <summary>

        /// Counter DAL - Get All Counters

        /// Description:- Retrieves all counter records from the database with pagination and search functionality.

        /// </summary>

        public async Task<APIGetResponseModel<List<CounterModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<CounterModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_CounterId", null);

                param.Add("p_BranchId", null);

                param.Add("p_CounterName", null);

                param.Add("p_CounterNumber", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", request.SearchKey);

                param.Add("p_PageNo", request.PageNo);

                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_counter", param, commandType: CommandType.StoredProcedure);

                // FIRST RESULT = DATA

                //var list = (await multi.ReadAsync<CounterModel>()).ToList();

                // SECOND RESULT = TOTAL

                //response.TotalRecords = await multi.ReadFirstAsync<int>();

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<CounterModel>()).ToList();

                response.Result = list;

                response.Result = list;

                response.IsSuccess = true;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching counters");

                Console.WriteLine("DAL COUNTER GET ALL ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// Counter DAL - Get Counter By Id

        /// Description:- Retrieves the details of a specific counter from the database using the counter Id.

        /// </summary>

        public async Task<APIGetResponseModel<CounterModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<CounterModel>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");

                param.Add("p_CounterId", id);

                param.Add("p_BranchId", null);

                param.Add("p_CounterName", null);

                param.Add("p_CounterNumber", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<CounterModel>("sp_manage_counter", param, commandType: CommandType.StoredProcedure);

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

                response.ErrorMsgs.Add("Error while fetching counter");

                Console.WriteLine("DAL COUNTER GETBYID ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// Counter DAL - Create Counter

        /// Description:- Inserts a new counter record into the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(CounterRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_CounterId", null);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_CounterName", request.CounterName);

                param.Add("p_CounterNumber", request.CounterNumber);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_counter", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting counter");

                Console.WriteLine("DAL COUNTER INSERT ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // UPDATE

        // ========================

        /// <summary>

        /// Counter DAL - Update Counter

        /// Description:- Updates the existing counter information in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Update(CounterRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");

                param.Add("p_CounterId", request.CounterId);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_CounterName", request.CounterName);

                param.Add("p_CounterNumber", request.CounterNumber);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_counter", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while updating counter");

                Console.WriteLine("DAL COUNTER UPDATE ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // STATUS

        // ========================

        /// <summary>

        /// Counter DAL - Change Counter Status

        /// Description:- Updates the active or inactive status of the specified counter in the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");

                param.Add("p_CounterId", id);

                param.Add("p_BranchId", null);

                param.Add("p_CounterName", null);

                param.Add("p_CounterNumber", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_counter", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while changing counter status");

                Console.WriteLine("DAL COUNTER STATUS ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // DROPDOWN

        // ========================

        /// <summary>

        /// Counter DAL - Get Counter Dropdown

        /// Description:- Retrieves the counter dropdown list from the database for UI selection controls.

        /// </summary>

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<DropdownModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");

                param.Add("p_CounterId", null);

                param.Add("p_BranchId", null);

                param.Add("p_CounterName", null);

                param.Add("p_CounterNumber", null);

                param.Add("p_Status", null);

                param.Add("p_SearchKey", null);

                param.Add("p_PageNo", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_counter", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = data.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching counter dropdown");

                Console.WriteLine("DAL COUNTER DROPDOWN ERROR: " + ex.Message);

            }

            return response;

        }

    }

}
