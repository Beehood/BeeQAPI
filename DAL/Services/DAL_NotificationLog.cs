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

    public class DAL_NotificationLog : IDAL_NotificationLog

    {

        private readonly DBConnection _config;

        public DAL_NotificationLog(DBConnection config)

        {

            _config = config;

        }

        // ========================

        // GET ALL

        // ========================

        /// <summary>

        /// Notification Log DAL - Get All Notification Logs

        /// Description:- Retrieves all notification log records from the database with pagination and search functionality.

        /// </summary>

        public async Task<APIGetResponseModel<List<NotificationLogModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<List<NotificationLogModel>>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "LIST");

                param.Add("p_NotificationId", null);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_TokenId", null);

                param.Add("p_CustomerId", null);

                param.Add("p_Recipient", null);

                param.Add("p_NotificationType", null);

                param.Add("p_TemplateCode", null);

                param.Add("p_Subject", null);

                param.Add("p_MessageBody", null);

                param.Add("p_ProviderName", null);

                param.Add("p_ProviderResponse", null);

                param.Add("p_Status", null);

                param.Add("p_ErrorMessage", null);

                param.Add("p_SentAt", null);
                param.Add("p_Email", email);
               

                using var multi = await conn.QueryMultipleAsync("sp_notification_logs", param, commandType: CommandType.StoredProcedure);

                var list = (await multi.ReadAsync<NotificationLogModel>()).ToList();

                response.Result = list;

                response.TotalRecords = list.Count;

                response.IsSuccess = true;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while fetching notification logs");

                Console.WriteLine("DAL GET ALL ERROR : " + ex.Message);

            }

            return response;

        }

        // ========================

        // GET BY ID

        // ========================

        /// <summary>

        /// Notification Log DAL - Get Notification Log By Id

        /// Description:- Retrieves notification log details using Notification Id.

        /// </summary>

        public async Task<APIGetResponseModel<NotificationLogModel>> GetById(long id, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<NotificationLogModel>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);


                var param = new DynamicParameters();


                param.Add("p_Action", "GETBYID");

                param.Add("p_NotificationId", id);

                param.Add("p_OrganizationId", null);

                param.Add("p_BranchId", null);

                param.Add("p_TokenId", null);

                param.Add("p_CustomerId", null);

                param.Add("p_Recipient", null);

                param.Add("p_NotificationType", null);

                param.Add("p_TemplateCode", null);

                param.Add("p_Subject", null);

                param.Add("p_MessageBody", null);

                param.Add("p_ProviderName", null);

                param.Add("p_ProviderResponse", null);

                param.Add("p_Status", null);

                param.Add("p_ErrorMessage", null);

                param.Add("p_SentAt", null);
                param.Add("p_Email", email);


                using var multi = await conn.QueryMultipleAsync("sp_notification_logs", param, commandType: CommandType.StoredProcedure);


                var data = (await multi.ReadAsync<NotificationLogModel>()).FirstOrDefault();

                if (data != null)

                {

                    response.Result = data;

                    response.TotalRecords = 1;

                    response.IsSuccess = true;

                }

                else

                {

                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Notification not found");

                }

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);


                Console.WriteLine("DAL GET BY ID ERROR : " + ex.Message);

            }


            return response;

        }

        // ========================

        // INSERT

        // ========================

        /// <summary>

        /// Notification Log DAL - Create Notification Log

        /// Description:- Inserts a new notification log record into the database.

        /// </summary>

        public async Task<APIGetResponseModel<int>> Insert(NotificationLogRequestDto request, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<int>();

            try

            {

                using var conn = new MySqlConnection(_config.DefaultConnection);

                var param = new DynamicParameters();

                param.Add("p_Action", "INSERT");

                param.Add("p_NotificationId", null);

                param.Add("p_OrganizationId", request.OrganizationId);

                param.Add("p_BranchId", request.BranchId);

                param.Add("p_TokenId", request.TokenId);

                param.Add("p_CustomerId", request.CustomerId);

                param.Add("p_Recipient", request.Recipient);

                param.Add("p_NotificationType", request.NotificationType);

                param.Add("p_TemplateCode", request.TemplateCode);

                param.Add("p_Subject", request.Subject);

                param.Add("p_MessageBody", request.MessageBody);

                param.Add("p_ProviderName", request.ProviderName);

                param.Add("p_ProviderResponse", request.ProviderResponse);

                param.Add("p_Status", request.Status);

                param.Add("p_ErrorMessage", request.ErrorMessage);

                param.Add("p_SentAt", request.SentAt);
                param.Add("p_Email", email);

                var id = await conn.ExecuteScalarAsync<long>("sp_notification_logs", param, commandType: CommandType.StoredProcedure);

                response.Result = (int)id;

                response.IsSuccess = id > 0;

                response.TotalRecords = id > 0 ? 1 : 0;

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                response.ErrorMsgs.Add("Error while inserting notification log");

                Console.WriteLine("DAL INSERT ERROR : " + ex.Message);

            }

            return response;

        }

    }

}

