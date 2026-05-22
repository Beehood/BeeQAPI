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
    public class BAL_DisplayBoard : IBAL_DisplayBoard
    {
        private readonly IDAL_DisplayBoard _dal;

        public BAL_DisplayBoard(IDAL_DisplayBoard dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<DisplayBoardModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                // 🔐 ROLE CHECK
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
                 || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase)||
                    r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase) ||
     r.Equals("Counter Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    return new APIGetResponseModel<List<DisplayBoardModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied. Only Super Admin allowed." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in DisplayBoard GetAll", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<DisplayBoardModel>> GetById(
            long id,
            List<string> roles,
            string email,
            IDbTransaction? transaction = null)
        {
            try
            {
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)
                  || r.Equals("Org Admin", StringComparison.OrdinalIgnoreCase) || r.Equals("Branch Admin", StringComparison.OrdinalIgnoreCase) ||
    r.Equals("Counter Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    return new APIGetResponseModel<DisplayBoardModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in DisplayBoard GetById", ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        public async Task<APIGetResponseModel<int>> Create(
            DisplayBoardRequestDto request,
            List<string> roles,
            string email,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // 🔐 ROLE CHECK
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can create display board.");
                    return response;
                }

                // ✅ VALIDATION
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.DisplayName))
                    response.ErrorMsgs.Add("Display Name is required");

                if (string.IsNullOrWhiteSpace(request.ScreenCode))
                    response.ErrorMsgs.Add("Screen Code is required");

                if (request.BranchId <= 0)
                    response.ErrorMsgs.Add("Branch is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                // 📦 CALL DAL
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
        public async Task<APIGetResponseModel<int>> Update(
            DisplayBoardRequestDto request,
            List<string> roles,
            string email,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can update display board.");
                    return response;
                }

                if (request == null || request.DisplayId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid display board data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.DisplayName))
                    response.ErrorMsgs.Add("Display Name is required");

                if (string.IsNullOrWhiteSpace(request.ScreenCode))
                    response.ErrorMsgs.Add("Screen Code is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
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
        // CHANGE STATUS
        // ========================
        public async Task<APIGetResponseModel<int>> ChangeStatus(
            long id,
            List<string> roles,
            string email,
            IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                Console.WriteLine("=== DISPLAY BOARD STATUS START ===");

                if (!roles.Any(r => r.Equals("Super Admin", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("❌ ROLE CHECK FAILED");
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can change status.");
                    return response;
                }

                if (id <= 0)
                {
                    Console.WriteLine("❌ INVALID DISPLAY ID");
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid display board ID.");
                    return response;
                }

                Console.WriteLine("➡️ CALLING DAL...");

                response = await _dal.ChangeStatus(id, email, transaction: localtran);

                Console.WriteLine("⬅️ DAL RESPONSE RECEIVED");
                Console.WriteLine("Result: " + response.Result);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ BAL DISPLAY ERROR:");
                Console.WriteLine(ex.Message);

                if (transaction == null && localtran != null)
                    localtran.Rollback();

                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            Console.WriteLine("=== DISPLAY BOARD STATUS END ===");

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(
            string email,
            IDbTransaction? transaction = null)
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
        public async Task<List<QueueDisplayModel>> GetDisplayData(string screenCode)
        {
            return await _dal.GetDisplayData(screenCode);
        }
    }
}