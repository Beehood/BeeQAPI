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

    public class DAL_DisplayBoardService : IDAL_DisplayBoardService

    {

        private readonly DBConnection _config;

        public DAL_DisplayBoardService(DBConnection config)

        {
            _config = config;
        }

        // ========================

        // LIST (GET SERVICES BY DISPLAY)

        // ========================

        /// <summary>

        /// Display Board Service DAL - Get All Display Board Services

        /// Description:- Retrieves all service mappings associated with the specified display board from the database.

        /// </summary>

        public async Task<APIGetResponseModel<List<DisplayBoardServiceModel>>> GetAll(long displayId, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<DisplayBoardServiceModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_Id", null);

                param.Add("p_DisplayId", displayId);

                param.Add("p_BranchServiceId", null);

                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DisplayBoardServiceModel>("sp_manage_display_board_service", param, commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;

                response.TotalRecords = data.Count;

                response.IsSuccess = data.Any();

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching display board services");

                Console.WriteLine("DAL DISPLAY SERVICE LIST ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// Display Board Service DAL - Create Display Board Service

        /// Description:- Inserts a new display board service mapping into the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(DisplayBoardServiceRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_Id", null);

                param.Add("p_DisplayId", request.DisplayId);

                param.Add("p_BranchServiceId", request.BranchServiceId);

                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_display_board_service", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting display board service");

                Console.WriteLine("DAL DISPLAY SERVICE INSERT ERROR: " + ex.Message);

            }

            return response;

        }

        // ========================

        // DELETE

        // ========================

        /// <summary>

        /// Display Board Service DAL - Delete Display Board Service

        /// Description:- Removes the specified display board service mapping from the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Delete(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DELETE");

                param.Add("p_Id", id);

                param.Add("p_DisplayId", null);

                param.Add("p_BranchServiceId", null);

                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_display_board_service", param, commandType: CommandType.StoredProcedure);

                response.Result = result;

                response.IsSuccess = result > 0;

                response.TotalRecords = result > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while deleting display board service");

                Console.WriteLine("DAL DISPLAY SERVICE DELETE ERROR: " + ex.Message);

            }

            return response;

        }

    }

}

