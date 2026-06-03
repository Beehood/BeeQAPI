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
    public class DAL_BranchDevice : IDAL_BranchDevice
    {
        private readonly DBConnection _config;

        public DAL_BranchDevice(DBConnection config)
        {
            _config = config;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<DeviceModel>>> GetAll(PaginationRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DeviceModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");
                param.Add("p_DeviceId", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_DeviceType", null);
                param.Add("p_DeviceName", null);
                param.Add("p_IpAddress", null);
                param.Add("p_MacAddress", null);
                param.Add("p_SearchKey", request.SearchKey);
                param.Add("p_PageNo", request.PageNo);
                param.Add("p_UserEmail", email);

                using var multi = await conn.QueryMultipleAsync("sp_manage_device",param,commandType: CommandType.StoredProcedure);

                response.TotalRecords = await multi.ReadFirstAsync<int>();

                var list = (await multi.ReadAsync<DeviceModel>()).ToList();

                response.Result = list;
                response.IsSuccess = list.Any();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching devices");
                Console.WriteLine(ex.Message);
            }

            return response;
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<DeviceModel>> GetById(long id,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<DeviceModel>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETBYID");
                param.Add("p_DeviceId", id);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_DeviceType", null);
                param.Add("p_DeviceName", null);
                param.Add("p_IpAddress", null);
                param.Add("p_MacAddress", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = await conn.QueryFirstOrDefaultAsync<DeviceModel>("sp_manage_device",param,commandType: CommandType.StoredProcedure);

                if (data != null)
                {
                    response.Result = data;
                    response.TotalRecords = 1;
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while fetching device");
                Console.WriteLine(ex.Message);
            }

            return response;
        }

        // ========================
        // INSERT
        // ========================
        public async Task<APIGetResponseModel<int>> Insert(DeviceRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");
                param.Add("p_DeviceId", null);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_DeviceType", request.DeviceType);
                param.Add("p_DeviceName", request.DeviceName);
                param.Add("p_IpAddress", request.IpAddress);
                param.Add("p_MacAddress", request.MacAddress);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_device",param,commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = id > 0 ? 1 : 0;
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
        public async Task<APIGetResponseModel<int>> Update(DeviceRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "UPDATE");
                param.Add("p_DeviceId", request.DeviceId);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_DeviceType", request.DeviceType);
                param.Add("p_DeviceName", request.DeviceName);
                param.Add("p_IpAddress", request.IpAddress);
                param.Add("p_MacAddress", request.MacAddress);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_manage_device",param,commandType: CommandType.StoredProcedure);

                response.Result = (int)id;
                response.IsSuccess = id > 0;
                response.TotalRecords = 1;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // STATUS
        // ========================
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "STATUS");
                param.Add("p_DeviceId", id);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_DeviceType", null);
                param.Add("p_DeviceName", null);
                param.Add("p_IpAddress", null);
                param.Add("p_MacAddress", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", 1);
                param.Add("p_UserEmail", email);

                var result = await conn.ExecuteScalarAsync<int>("sp_manage_device",param,commandType: CommandType.StoredProcedure);

                response.Result = result;
                response.IsSuccess = true;
                response.TotalRecords = 1;
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
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "DROPDOWN");
                param.Add("p_DeviceId", null);
                param.Add("p_OrganizationId", null);
                param.Add("p_BranchId", null);
                param.Add("p_DeviceType", null);
                param.Add("p_DeviceName", null);
                param.Add("p_IpAddress", null);
                param.Add("p_MacAddress", null);
                param.Add("p_SearchKey", null);
                param.Add("p_PageNo", null);
                param.Add("p_UserEmail", email);

                var data = (await conn.QueryAsync<DropdownModel>("sp_manage_device",param,commandType: CommandType.StoredProcedure)).ToList();

                response.Result = data;
                response.TotalRecords = data.Count;
                response.IsSuccess = data.Any();
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