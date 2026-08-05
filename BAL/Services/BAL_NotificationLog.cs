using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class BAL_NotificationLog : IBAL_NotificationLog
    {
        private readonly IDAL_NotificationLog _dal;

        public BAL_NotificationLog(IDAL_NotificationLog dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Notification Log API - Get All Notification Logs
        /// Author: Swapnalisa
        /// Description:- Fetch notification log list.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<List<NotificationLogModel>>> GetAll(PaginationRequestDto request,List<string> roles, string? email,IDbTransaction? transaction = null)
        {
            try
            {
                // ROLE CHECK
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<NotificationLogModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string>
                {
                    "Access denied."
                }
                    };
                }

                // USER CHECK
                if (string.IsNullOrWhiteSpace(email))
                {
                    return new APIGetResponseModel<List<NotificationLogModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string>
                {
                    "Unable to identify logged-in user."
                }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetAll (NotificationLog)",ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Notification Log API - Get Notification Log By Id
        /// Description:- Fetch notification log details by Notification Id.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<NotificationLogModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<NotificationLogModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string>
                        {
                            "Access denied."
                        }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetById (NotificationLog)", ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Notification Log API - Create Notification Log
        /// Description:- Validates notification data and stores notification history.
        /// Access:
        /// - System Generated
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Create(NotificationLogRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // VALIDATION
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Recipient))
                    response.ErrorMsgs.Add("Recipient is required.");

                if (string.IsNullOrWhiteSpace(request.NotificationType))
                    response.ErrorMsgs.Add("Notification Type is required.");

                if (string.IsNullOrWhiteSpace(request.MessageBody))
                    response.ErrorMsgs.Add("Message Body is required.");

                if (string.IsNullOrWhiteSpace(request.Status))
                    response.ErrorMsgs.Add("Status is required.");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                response = await _dal.Insert(request, email, transaction: localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception ex)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }
    }
}
