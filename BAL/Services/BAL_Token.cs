using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BAL.Services

{

    public class BAL_Token : IBAL_Token

    {

        private readonly IDAL_Token _dal;

        private readonly ILogger<BAL_Token> _logger;

        public BAL_Token(IDAL_Token dal, ILogger<BAL_Token> logger)

        {

            _dal = dal;

            _logger = logger;

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

                _logger.LogError(

                    ex,

                    "BAL GetAll Error");

                return new APIGetResponseModel<List<TokenModel>>

                {

                    IsSuccess = false,

                    ErrorMsgs = new List<string>

        {

            "Error while fetching tokens"

        }

                };

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

                _logger.LogError(

                    ex,

                    "BAL GetById Error");

                return new APIGetResponseModel<TokenModel>

                {

                    IsSuccess = false,

                    ErrorMsgs = new List<string>

        {

            "Error while fetching token"

        }

                };

            }

        }

        // ========================

        // GENERATE TOKEN

        // ========================

        /// <summary>

        /// Token API - Generate Token

        /// Description:- Creates new token using sp_generate_token

        /// </summary>

        public async Task<APIGetResponseModel<string>> GenerateToken(TokenRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)

        {

            var response = new APIGetResponseModel<string>();

            IDbTransaction? localtran = null;

            try

            {

                _logger.LogInformation("GenerateToken requested by {Email} for BranchServiceId {BranchServiceId}", email, request?.BranchServiceId);

                if (!roles.Any())

                {

                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    _logger.LogWarning("Unauthorized GenerateToken attempt by {Email}", email);

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

                if (response.ErrorMsgs.Any())

                {

                    response.IsSuccess = false;

                    return response;

                }

                response = await _dal.GenerateToken(request, email, transaction: localtran);

                if (response.IsSuccess)

                {

                    _logger.LogInformation(

                        "Token generated successfully for {Email}", email);

                }

                if (transaction == null && localtran != null)

                    localtran.Commit();

            }

            catch (Exception ex)

            {

                if (transaction == null && localtran != null)

                    localtran.Rollback();

                response.IsSuccess = false;

                _logger.LogError(ex, "BAL GenerateToken Error");

                response.ErrorMsgs.Add("Error while generating token");

                return response;

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

                _logger.LogError(ex, "BAL ChangeStatus Error");

                response.ErrorMsgs.Add("Error while updating token status");

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

                _logger.LogInformation("CallNextToken requested by {Email}", email);

                if (!roles.Any())

                {

                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    _logger.LogWarning("Unauthorized CallNextToken attempt by {Email}", email);

                    return response;

                }

                if (request == null)

                {

                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Invalid payload.");

                    return response;

                }

                response = await _dal.CallNextToken(request, email, transaction);

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                _logger.LogError(ex, "BAL CallNextToken Error");

                response.ErrorMsgs.Add("Error while calling next token");

            }



            return response;

        }

        public async Task
<APIGetResponseModel<List<TokenStatusModel>>> GetStatuses(string email, IDbTransaction? transaction = null)

        {

            return await _dal.GetStatuses(email, transaction);

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

                _logger.LogError(ex, "BAL GetDropdown Error");

                response.ErrorMsgs.Add("Error while fetching dropdown");

            }

            return response;

        }

        public async Task<APIGetResponseModel<TokenModel>> NextTokenPreview(TokenRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)

        {

            var response =

                new APIGetResponseModel<TokenModel>();

            try

            {

                _logger.LogInformation(

                    "NextTokenPreview requested by {Email}",

                    email);

                if (!roles.Any())

                {

                    response.IsSuccess = false;

                    response.ErrorMsgs.Add(

                        "Access denied.");

                    _logger.LogWarning(

                        "Unauthorized NextTokenPreview attempt by {Email}",

                        email);

                    return response;

                }

                if (request == null)

                {

                    response.IsSuccess = false;

                    response.ErrorMsgs.Add(

                        "Invalid payload.");

                    return response;

                }

                response = await _dal.NextTokenPreview(

                    request,

                    roles,

                    email,

                    transaction);

                if (response.IsSuccess)

                {

                    _logger.LogInformation(

                        "NextTokenPreview completed successfully for {Email}",

                        email);

                }

            }

            catch (Exception ex)

            {

                response.IsSuccess = false;

                _logger.LogError(

                    ex,

                    "BAL NextTokenPreview Error");

                response.ErrorMsgs.Add(

                    "Error while previewing next token");

            }

            return response;

        }

    }

}

