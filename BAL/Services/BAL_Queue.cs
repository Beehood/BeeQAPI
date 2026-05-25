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
    public class BAL_Queue : IBAL_Queue
    {
        private readonly IDAL_Queue _dal;

        public BAL_Queue(IDAL_Queue dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<QueueModel>>> GetAll(PaginationRequestDto request,List<string> roles, string? email,IDbTransaction? transaction = null)
        {
            try
            {
                // Example Role Check
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
                 || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase) ||
                    r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    return new APIGetResponseModel<List<QueueModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetAll", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<QueueModel>> GetById( long tokenId,List<string> roles,string email, IDbTransaction? transaction = null)
        {
            try
            {
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
               || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase) ||
                  r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    return new APIGetResponseModel<QueueModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(tokenId, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in GetById", ex);
            }
        }

        // ========================
        // CREATE TOKEN
        // ========================
        public async Task<APIGetResponseModel<int>> Create(QueueRequestDto request,List<string> roles,string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase) ||
                   r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Admin can create token.");
                    return response;
                }

                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (request.BranchId <= 0)
                    response.ErrorMsgs.Add("Branch is required");

                if (request.BranchServiceId <= 0)
                    response.ErrorMsgs.Add("Service is required");

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

        // ========================
        // UPDATE
        // ========================
        public async Task<APIGetResponseModel<int>> Update(QueueRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase) ||
                   r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Admin can update queue.");
                    return response;
                }

                if (request == null || request.TokenId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid token data.");
                    return response;
                }

                response = await _dal.Update(request, email, transaction: localtran);

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

        // ========================
        // STATUS (CALL / COMPLETE / TRANSFER)
        // ========================
        public async Task<APIGetResponseModel<int>> ChangeStatus(QueueRequestDto request, List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
                || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase) ||
                   r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Unauthorized access.");
                    return response;
                }

                if (request.TokenId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid token ID.");
                    return response;
                }

                response = await _dal.ChangeStatus(request, email, transaction: localtran);

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

        // ========================
        // QUEUE DISPLAY (MONITOR)
        // ========================
        public async Task<APIGetResponseModel<List<QueueDisplayModel>>> GetQueueDisplay(string branchId)
        {
            var response = new APIGetResponseModel<List<QueueDisplayModel>>();

            try
            {
                response = await _dal.GetQueueDisplay(branchId);
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
                response = await _dal.GetDropdown(email, transaction);
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
