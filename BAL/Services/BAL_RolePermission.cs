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
    public class BAL_RolePermission : IBAL_RolePermission
    {
        private readonly IDAL_RolePermission _dal;

        public BAL_RolePermission(IDAL_RolePermission dal)
        {
            _dal = dal;
        }

        // ========================
        // GET ALL
        // ========================
        public async Task<APIGetResponseModel<List<RolePermissionModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                //  All roles can view (SP will filter data)
                if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<RolePermissionModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetAll(request, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in RolePermission GetAll", ex);
            }
        }

        // ========================
        // GET BY ROLE
        // ========================
        public async Task<APIGetResponseModel<List<RolePermissionModel>>> GetByRoleId(long roleId,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") || roles.Contains("Org Admin") ||roles.Contains("Branch Admin")))
                {
                    return new APIGetResponseModel<List<RolePermissionModel>>
                    {
                        IsSuccess = false,
                        ErrorMsgs = new List<string> { "Access denied." }
                    };
                }

                return await _dal.GetByRoleId(roleId, email, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("BAL: Error in RolePermission GetByRoleId", ex);
            }
        }

        // ========================
        // CREATE (Single Assign)
        // ========================
        public async Task<APIGetResponseModel<int>> Create(RolePermissionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  Only Super Admin can assign permission
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can assign permissions.");
                    return response;
                }

                if (request == null || request.RoleId <= 0 || request.PermissionId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid role permission data.");
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
        // BULK ASSIGN (UPDATE)
        // ========================
        public async Task<APIGetResponseModel<int>> BulkAssign(RolePermissionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                // Only Super Admin can assign permissions
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can assign permissions.");
                    return response;
                }

                if (request == null || request.RoleId <= 0 || string.IsNullOrWhiteSpace(request.PermissionIds))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid bulk permission data.");
                    return response;
                }

                response = await _dal.BulkInsert(request, email, transaction: localtran);

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
        // DELETE
        // ========================
        public async Task<APIGetResponseModel<int>> Delete(RolePermissionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<int>();
            IDbTransaction? localtran = null;

            try
            {
                //  Only Super Admin can remove permission
                if (!roles.Contains("Super Admin"))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Only Super Admin can remove permissions.");
                    return response;
                }

                if (request == null || request.RoleId <= 0 || request.PermissionId <= 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Invalid role permission data.");
                    return response;
                }

                response = await _dal.Delete(request, email, transaction: localtran);

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
