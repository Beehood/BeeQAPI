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
    public class DAL_Report : IDAL_Report
    {
        private readonly DBConnection _config;

        public DAL_Report(DBConnection config)
        {
            _config = config;
        }
        /// <summary>
        /// Report DAL - Get All Reports
        /// Description:- Retrieves report data from the database based on the selected report type and filter criteria.
        /// </summary>
        public async Task<APIGetResponseModel<List<ReportModel>>> GetAll(ReportRequestDto request,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<ReportModel>>();

            try
            {
                using var conn = new MySqlConnection(_config.DefaultConnection);

                await conn.OpenAsync();

                var param = new DynamicParameters();

                param.Add("p_Action", request.Action);
                param.Add("p_OrganizationId", request.OrganizationId);
                param.Add("p_BranchId", request.BranchId);
                param.Add("p_FromDate", request.FromDate);
                param.Add("p_ToDate", request.ToDate);

                var result = (await conn.QueryAsync<ReportModel>("SP_REPORTS",param,commandType: CommandType.StoredProcedure)).ToList();

                response.Result = result;
                response.TotalRecords = result.Count;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);

                Console.WriteLine($"REPORT ERROR : {ex.Message}");
            }

            return response;
        }
    }
}