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
{  /// <summary>
   /// Permission API - BAL Layer
   /// Author: Swapnlisa
   /// Description:- Handles permission business logic with RBAC access control.
   /// </summary>
    public class BAL_Permission : IBAL_Permission
    {
        private readonly IDAL_Permission _dal;

        public BAL_Permission(IDAL_Permission dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<PermissionModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                // RBAC: All roles can view based on SP filtering
                if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<PermissionModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in Permission GetAll", ex);
            }
        }

        // ========================
        // GET BY ID
        // ========================
        public async Task<APIGetResponseModel<PermissionModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<PermissionModel>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetById(id, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in Permission GetById", ex);
            }
        }

        // ========================
        // CREATE
        // ========================
        public async Task<APIGetResponseModel<int>> Create(PermissionRequestDto request,List<string> roles,  string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // Only Super Admin can create permissions
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can create permissions.");
                    return response;
                }

                //  VALIDATION
                if (request == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid payload.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.PermissionName))
                    response.ErrorMsgs.Add("Permission Name is required");

                if (string.IsNullOrWhiteSpace(request.PermissionCode))
                    response.ErrorMsgs.Add("Permission Code is required");

                if (response.ErrorMsgs.Any())
                {
                    response.IsSuccess = false;
                    return response;
                }

                //  CALL DAL
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
        public async Task<APIGetResponseModel<int>> Update(PermissionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  Only Super Admin can update permissions
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can update permissions.");
                    return response;
                }

                if (request == null || request.PermissionId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid permission data.");
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.PermissionName))
                    response.ErrorMsgs.Add("Permission Name is required");

                if (string.IsNullOrWhiteSpace(request.PermissionCode))
                    response.ErrorMsgs.Add("Permission Code is required");

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
        public async Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  Only Super Admin can change permission status
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can change permission status.");
                    return response;
                }

                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid permission ID.");
                    return response;
                }

                response = await _dal.ChangeStatus(id, email, transaction: localtran);

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
