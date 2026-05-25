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
    public class BAL_DisplayBoardService : IBAL_DisplayBoardService
    {
        private readonly IDAL_DisplayBoardService _dal;

        public BAL_DisplayBoardService(IDAL_DisplayBoardService dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL (SERVICES BY DISPLAY)
        // ========================
        public async Task<APIGetResponseModel<List<DisplayBoardServiceModel>>> GetAll(long displayId,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                // ROLE CHECK
                if (!roles.Any(r =>
                    r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase) ||
                    r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase) ||
                    r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    return new APIGetResponseModel<List<DisplayBoardServiceModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                if (displayId <= 0)
                {
                    return new APIGetResponseModel<List<DisplayBoardServiceModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Invalid Display Id." }
                    };
                }

                return await _dal.GetAll(displayId, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in DisplayBoardService GetAll", ex);
            }
        }

        // ========================
        // CREATE (MAP SERVICE)
        // ========================
        public async Task<APIGetResponseModel<int>> Create( DisplayBoardServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // ROLE CHECK
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can map services.");
                    return response;
                }

                //  VALIDATION
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (request.DisplayId <= 0)
                    response.ErrorMsgs.Add("Display Id is required");

                if (request.BranchServiceId <= 0)
                    response.ErrorMsgs.Add("Branch Service Id is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                // CALL DAL
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
        // DELETE (REMOVE MAPPING)
        // ========================
        public async Task<APIGetResponseModel<int>> Delete(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // ROLE CHECK
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can delete mapping.");
                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid mapping Id.");
                    return response;
                }

                // CALL DAL
                response = await _dal.Delete(id, email, transaction: localtran);

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
