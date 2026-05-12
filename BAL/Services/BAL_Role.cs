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
   
        public class BAL_Role : IBAL_Role
        {
            private readonly IDAL_Role _dal;

            public BAL_Role(IDAL_Role dal)
            {
                _dal = dal;
            }

            // ========================
            // GET ALL
            // ========================

            public async Task<APIGetResponseModel<List<RoleModel>>> GetAll(PaginationRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null)
            {
                try
                {
                    // ROLE CHECK

                    if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin")))
                    {
                        return new APIGetResponseModel<List<RoleModel>>
                        {
                            IsSuccess = false,
                            ErrorMsgs = new List<string>
                        {
                            "Access denied."
                        }
                        };
                    }

                    return await _dal.GetAll(request, email, transaction);
                }
                catch (Exception ex)
                {
                    throw new Exception("BAL: Error in Role GetAll", ex);
                }
            }

            // ========================
            // GET BY ID
            // ========================

            public async Task<APIGetResponseModel<RoleModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null)
            {
                try
                {
                    if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin")))
                    {
                        return new APIGetResponseModel<RoleModel>
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
                    throw new Exception("BAL: Error in Role GetById", ex);
                }
            }

            // ========================
            // CREATE
            // ========================

            public async Task<APIGetResponseModel<int>> Create(RoleRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
            {
                var response = new APIGetResponseModel<int>();

                IDbTransaction? localtran = null;

                try
                {
                    // ROLE CHECK

                    if (!(roles.Contains("Super Admin")))
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

                    if (string.IsNullOrWhiteSpace(request.RoleName))
                        response.ErrorMsgs.Add("Role name is required");

                    if (string.IsNullOrWhiteSpace(request.RoleCode))
                        response.ErrorMsgs.Add("Role code is required");

                    if (response.ErrorMsgs.Any())
                    {
                        response.IsSuccess = false;

                        return response;
                    }

                    // DAL CALL

                    response = await _dal.Insert(request, email, transaction: localtran);

                    if (transaction == null && localtran != null)
                    {
                        localtran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction == null && localtran != null)
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

            public async Task<APIGetResponseModel<int>> Update(RoleRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null)
            {
                var response = new APIGetResponseModel<int>();

                IDbTransaction? localtran = null;

                try
                {
                    if (!(roles.Contains("Super Admin")))
                    {
                        response.IsSuccess = false;

                        response.ErrorMsgs.Add("Access denied.");

                        return response;
                    }

                    if (request == null || request.RoleId <= 0)
                    {
                        response.IsSuccess = false;

                        response.ErrorMsgs.Add("Invalid role data.");

                        return response;
                    }

                    if (string.IsNullOrWhiteSpace(request.RoleName))
                        response.ErrorMsgs.Add("Role name is required");

                    if (string.IsNullOrWhiteSpace(request.RoleCode))
                        response.ErrorMsgs.Add("Role code is required");

                    if (response.ErrorMsgs.Any())
                    {
                        response.IsSuccess = false;

                        return response;
                    }

                    response = await _dal.Update(request, email, transaction: localtran);

                    if (transaction == null && localtran != null)
                    {
                        localtran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction == null && localtran != null)
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

            public async Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email, IDbTransaction? transaction = null)
            {
                var response = new APIGetResponseModel<int>();

                IDbTransaction? localtran = null;

                try
                {
                    if (!(roles.Contains("Super Admin")))
                    {
                        response.IsSuccess = false;

                        response.ErrorMsgs.Add("Access denied.");

                        return response;
                    }

                    if (id <= 0)
                    {
                        response.IsSuccess = false;

                        response.ErrorMsgs.Add("Invalid role ID.");

                        return response;
                    }

                    response = await _dal.ChangeStatus(id, email, transaction: localtran);

                    if (transaction == null && localtran != null)
                    {
                        localtran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction == null && localtran != null)
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