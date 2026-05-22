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
    public class DAL_DisplayBoard : IDAL_DisplayBoard
    {
        private readonly DBConnection _config;

        public DAL_DisplayBoard(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<DisplayBoardModel>>> GetAll(PaginationRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DisplayBoardModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "LIST");
                param.Add("p_DisplayId", null);
                param.Add("p_BranchId", null);
                param.Add("p_DisplayName", null);
                param.Add("p_ScreenCode", null);
                param.Add("p_UpcomingLimit", null);
                param.Add("p_TemplateId", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_display_board",param,commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();
                var list = (await multi.ReadAsync<DisplayBoardModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching display boards");
                Console.WriteLine("DAL DISPLAY GET ALL ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<DisplayBoardModel>> GetById(long id,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<DisplayBoardModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "GETBYID");
                param.Add("p_DisplayId", id);
                param.Add("p_BranchId", null);
                param.Add("p_DisplayName", null);
                param.Add("p_ScreenCode", null);
                param.Add("p_UpcomingLimit", null);
                param.Add("p_TemplateId", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<DisplayBoardModel>("sp_manage_display_board",param,commandType: CommandType.StoredProcedure);

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
                response.ErrorMsgs.Add("Error while fetching display board");
                Console.WriteLine("DAL DISPLAY GET BY ID ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        public async Task<APIGetResponseModel<int>> Insert(DisplayBoardRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "INSERT");
                param.Add("p_DisplayId", null);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_DisplayName", request.DisplayName);
                param.Add("p_ScreenCode", request.ScreenCode);
                param.Add("p_UpcomingLimit", request.UpcomingLimit);
                param.Add("p_TemplateId", request.TemplateId);
                param.Add("p_Status", 1);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_display_board",param,commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while inserting display board");
                Console.WriteLine("DAL DISPLAY INSERT ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<int>> Update(DisplayBoardRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "UPDATE");
                param.Add("p_DisplayId", request.DisplayId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_DisplayName", request.DisplayName);
                param.Add("p_ScreenCode", request.ScreenCode);
                param.Add("p_UpcomingLimit", request.UpcomingLimit);
                param.Add("p_TemplateId", request.TemplateId);
                param.Add("p_Status", request.Status ? 1 : 0);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_display_board",param,commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while updating display board");
                Console.WriteLine("DAL DISPLAY UPDATE ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "STATUS");
                param.Add("p_DisplayId", id);
                param.Add("p_BranchId", null);
                param.Add("p_DisplayName", null);
                param.Add("p_ScreenCode", null);
                param.Add("p_UpcomingLimit", null);
                param.Add("p_TemplateId", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", 1);
                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>(
                    "sp_manage_display_board",
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
                response.ErrorMsgs.Add("Error while changing display board status");
                Console.WriteLine("DAL DISPLAY STATUS ERROR: " + ex.Message);
            }

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();
                param.Add("p_Action", "DROPDOWN");
                param.Add("p_DisplayId", null);
                param.Add("p_BranchId", null);
                param.Add("p_DisplayName", null);
                param.Add("p_ScreenCode", null);
                param.Add("p_UpcomingLimit", null);
                param.Add("p_TemplateId", null);
                param.Add("p_Status", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_display_board",param,commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = data.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching display board dropdown");
                Console.WriteLine("DAL DISPLAY DROPDOWN ERROR: " + ex.Message);
            }

            return response;
        }
        public async Task<List<QueueDisplayModel>> GetDisplayData(string screenCode)
        {
            using var conn = new MySqlConnection(_config.DefaultConnection);

            var param = new DynamicParameters();
            param.Add("p_Action", "DISPLAY");
            param.Add("p_DisplayId", null);   // ✅ REQUIRED
            param.Add("p_BranchId", null);
            param.Add("p_DisplayName", null);
            param.Add("p_ScreenCode", screenCode);
            param.Add("p_UpcomingLimit", null);
            param.Add("p_TemplateId", null);
            param.Add("p_Status", null);
            param.Add("p_SearchKey", null);
            param.Add("p_PageNo", null);
            param.Add("p_UserEmail", null);

            var result = await conn.QueryAsync<QueueDisplayModel>(
                "sp_manage_display_board",
                param,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
    }
    
}

