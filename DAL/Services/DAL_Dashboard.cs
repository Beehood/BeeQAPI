using DAL.ContractIF;
using DAL.Dbcontext;
using Dapper;
using Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace DAL.Services
{
    public class DAL_Dashboard : IDAL_Dashboard
    {
        private readonly DBConnection _config;

        public DAL_Dashboard(DBConnection config)
        {
            _config = config;
        }

        public async Task<APIGetResponseModel<DashboardModel>> GetDashboard(DashboardRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<DashboardModel>();

            try
            {
                using var conn =new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "GETDASHBOARD");
                param.Add("p_UserEmail", email);
                param.Add("p_OrganizationId", request.OrganizationId);

                using var multi =await conn.QueryMultipleAsync("sp_dashboard",param,commandType: CommandType.StoredProcedure);

                var model = new DashboardModel();

                // Result Set 1
                model.Summary =await multi.ReadFirstOrDefaultAsync<DashboardSummaryModel>()?? new DashboardSummaryModel();

                // Result Set 2
                model.QueueStats =await multi.ReadFirstOrDefaultAsync<DashboardQueueModel>()?? new DashboardQueueModel();

                // Result Set 3
                model.QueueTrend =(await multi.ReadAsync<DashboardTrendModel>()).ToList();

                // Result Set 4
                model.TopBranches =(await multi.ReadAsync<DashboardBranchModel>()).ToList();

                // Result Set 5
                model.TopServices =(await multi.ReadAsync<DashboardServiceModel>()).ToList();

                // Result Set 6
                model.RecentActivities =(await multi.ReadAsync<DashboardActivityModel>()).ToList();

                response.Result = model;
                response.TotalRecords = 1;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add("Error while loading dashboard");

                Console.WriteLine("DAL DASHBOARD ERROR: " + ex.Message);
            }

            return response;
        }
    }
}