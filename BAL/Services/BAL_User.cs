using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class BAL_User : IBAL_User
    {
        private readonly IDAL_User _dal;

        public BAL_User(IDAL_User dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================

        public async Task<APIGetResponseModel<List<UserModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null )
        {
            try
            {
                // ROLE CHECK
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<UserModel>>
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
                throw new Exception("BAL: Error in User GetAll",ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================

        public async Task<APIGetResponseModel<UserModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<UserModel>{IsSuccess = false,ErrorMsgs = new List<string>{"Access denied."}
                    };
                }

                return await _dal.GetById(id,email,transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in User GetById",ex);
            }
        }

        // ========================
        // CREATE
        // ========================

        public async Task<APIGetResponseModel<int>> Create(UserRequestDto request,List<string> roles, string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<int>();

            IDbTransaction? localtran = null;

            try
            {
                // ROLE CHECK
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin")))
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                // VALIDATION

                if (request == null)
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Invalid payload.");

                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                    response.ErrorMsgs.Add("User Name is required");

                if (string.IsNullOrWhiteSpace(request.Email))
                    response.ErrorMsgs.Add("Email is required");

                //if (string.IsNullOrWhiteSpace(request.Phone))
                    //response.ErrorMsgs.Add("Phone is required");

                if (string.IsNullOrWhiteSpace(request.Password))
                    response.ErrorMsgs.Add("Password is required");

                if (request.RoleId <= 0)
                    response.ErrorMsgs.Add("Role is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;

                    return response;
                }
                request.Password =
                    BAL_Auth.GetHashString(request.Password);
                // DAL CALL

                response = await _dal.Insert(request,email,transaction: localtran);

                if (transaction == null &&localtran != null)
                {
                    localtran.Commit();
                }
            }
            catch (Exception ex)
            {
                if (transaction == null &&localtran != null)
                {
                    localtran.Rollback();
                }

                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // UPDATE
        // ========================

        public async Task<APIGetResponseModel<int>> Update(UserRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<int>();

            IDbTransaction? localtran = null;

            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin")))
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                if (request == null ||request.UserId <= 0)
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Invalid user data.");

                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                    response.ErrorMsgs.Add("User Name is required");

                if (string.IsNullOrWhiteSpace(request.Email))
                    response.ErrorMsgs.Add("Email is required");

                //if (string.IsNullOrWhiteSpace(request.Phone))
                    //response.ErrorMsgs.Add("Phone is required");

                if (request.RoleId <= 0)
                    response.ErrorMsgs.Add("Role is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;

                    return response;
                }
                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    if (!string.IsNullOrWhiteSpace(request.Password))
                    {
                        request.Password =
                            BAL_Auth.GetHashString(request.Password);
                    }
                }
                response = await _dal.Update( request,email,transaction: localtran);

                if (transaction == null &&localtran != null)
                {
                    localtran.Commit();
                }
            }
            catch (Exception ex)
            {
                if (transaction == null &&localtran != null)
                {
                    localtran.Rollback();
                }

                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // CHANGE STATUS
        // ========================

        public async Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<int>();

            IDbTransaction? localtran = null;

            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin")))
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Access denied.");

                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;

                    response.ErrorMsgs.Add("Invalid user ID.");

                    return response;
                }

                response = await _dal.ChangeStatus(id,email,transaction: localtran);

                if (transaction == null &&localtran != null)
                {
                    localtran.Commit();
                }
            }
            catch (Exception ex)
            {
                if (transaction == null &&localtran != null)
                {
                    localtran.Rollback();
                }

                response.IsSuccess = false;

                response.ErrorMsgs.Add(ex.Message);
            }

            return response;
        }

        // ========================
        // DROPDOWN
        // ========================

        public async Task<APIGetResponseModel<List<DropdownModel>>>GetDropdown(string email,IDbTransaction? transaction = null)
        {
            var response =new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                response =await _dal.GetDropdown(email, transaction );
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