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
    public class BAL_ActiveLog : IBAL_ActiveLog
    {
        private readonly IDAL_ActiveLog _dal;

        public BAL_ActiveLog(IDAL_ActiveLog dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Activity Log BAL - Get All Activity Logs
        /// Author: Swapnalisa
        /// Description:- Validates user role and fetches activity log list.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<List<ActivityLogModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin")
                    || roles.Contains("Org Admin")
                    || roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<ActivityLogModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string>
                    {
                        "Access denied."
                    }
                    };
                }

                return await _dal.GetAll(request,email,transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "BAL: Error in GetAll (Activity Log)",
                    ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Activity Log BAL - Get Activity Log By Id
        /// Description:- Validates user role and fetches activity log details.
        /// Access:
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<ActivityLogModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin")
                    || roles.Contains("Org Admin")
                    || roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<ActivityLogModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string>
                    {
                        "Access denied."
                    }
                    };
                }

                return await _dal.GetById(id,email,transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "BAL: Error in GetById (Activity Log)",
                    ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        /// <summary>
        /// Activity Log BAL - Create Activity Log
        /// Description:- Validates request and inserts activity log.
        /// Used for audit trail tracking.
        /// Access:
        /// - System Generated
        /// - Super Admin
        /// - Org Admin
        /// - Branch Admin
        /// </summary>
        public async Task<APIGetResponseModel<int>> Create(ActivityLogRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<int>();

            try
            {
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add(
                        "Invalid payload.");

                    return response;
                }

                response = await _dal.Insert(request,email,transaction);

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);

                return response;
            }
        }
    }

}
