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
    public class BAL_Token : IBAL_Token
    {
        private readonly IDAL_Token _dal;

        public BAL_Token(IDAL_Token dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        /// <summary>
        /// Token API - Get All Tokens
        /// Author: Swapnlisa
        /// Description:- Fetch token list with pagination.
        /// </summary>
        public async Task<APIGetResponseModel<List<TokenModel>>> GetAll(PaginationRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null)
        {
            try
            {
                if (!roles.Any())
                {
                    return new APIGetResponseModel<List<TokenModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in Token GetAll", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Token API - Get Token By Id
        /// </summary>
        public async Task<APIGetResponseModel<TokenModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            try
            {
                if (!roles.Any())
                {
                    return new APIGetResponseModel<TokenModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in Token GetById", ex);
            }
        }

        // ========================
        // GENERATE TOKEN
        // ========================
        /// <summary>
        /// Token API - Generate Token
        /// Description:- Creates new token using sp_generate_token
        /// </summary>
        public async Task<APIGetResponseModel<int>> GenerateToken(TokenRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (request.BranchServiceId <= 0)
                    response.ErrorMsgs.Add("Branch Service required");

                if (string.IsNullOrWhiteSpace(request.CustomerName))
                    response.ErrorMsgs.Add("Customer Name required");

                if (string.IsNullOrWhiteSpace(request.CustomerPhone))
                    response.ErrorMsgs.Add("Customer Phone required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                response = await _dal.GenerateToken(request, email, transaction: localtran);

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
        // STATUS
        // ========================
        /// <summary>
        /// Token API - Change Token Status
        /// </summary>
        public async Task<APIGetResponseModel<int>> ChangeStatus(TokenRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                if (request.TokenId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid TokenId.");
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
        // CALL NEXT TOKEN
        // ========================
        /// <summary>
        /// Token API - Call Next Token
        /// </summary>
        public async Task<APIGetResponseModel<TokenModel>> CallNextToken(TokenRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<TokenModel>();

            try
            {
                if (!roles.Any())
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                response = await _dal.CallNextToken(request, email, transaction);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        public async Task
       <APIGetResponseModel<List<TokenStatusModel>>>
        GetStatuses(string email,IDbTransaction? transaction = null)
        {
            return await _dal.GetStatuses(email,transaction);
        }

        // ========================
        // DROPDOWN
        // ========================
        /// <summary>
        /// Token API - Get Token Dropdown
        /// </summary>
        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null)
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
