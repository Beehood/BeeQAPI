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
        /// <summary>
        /// Fetch all roles (paginated)
        /// Access:
        /// Super Admin
        /// Org Admin
        /// </summary>

        public async Task<APIGetResponseModel<List<RoleModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null)
        {
            try
            {
                // =========================================
                // ACCESS CHECK
                // =========================================

                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
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


                // =========================================
                // GET DATA
                // =========================================

                var response = await _dal.GetAll(request,email,transaction);


                if (response.Result == null)
                {
                    response.Result = new List<RoleModel>();
                    response.TotalRecords = 0;

                    return response;
                }


                // =========================================
                // SUPER ADMIN
                // Can view all roles
                // =========================================

                if (roles.Contains("Super Admin"))
                {
                    // No filtering
                }


                // =========================================
                // ORG ADMIN
                // Cannot view Super Admin
                // =========================================

                //else if (roles.Contains("Org Admin"))
                //{
                //    response.Result = response.Result
                //        .Where(x =>!string.Equals(x.RoleName,"Super Admin",StringComparison.OrdinalIgnoreCase)).ToList();
                //}


                //// =========================================
                //// BRANCH ADMIN
                //// Cannot view Super Admin or Org Admin
                //// =========================================

                //else if (roles.Contains("Branch Admin"))
                //{
                //    response.Result = response.Result
                //        .Where(x =>!string.Equals(x.RoleName,"Super Admin",StringComparison.OrdinalIgnoreCase)&&!string.Equals(x.RoleName,"Org Admin",StringComparison.OrdinalIgnoreCase)).ToList();
                //}


                //response.TotalRecords = response.Result.Count;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "BAL: Error in Role GetAll",
                    ex
                );
            }
        }

        // ========================
        // GET BY ID
        // ========================
        /// <summary>
        /// Fetch role by ID
        /// Access:
        /// Super Admin
        /// Org Admin
        /// </summary>

        public async Task<APIGetResponseModel<RoleModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null)
        {
            try
            {
                if (!(roles.Contains("Super Admin") ||
                       roles.Contains("Org Admin") ||
                       roles.Contains("Branch Admin")))
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
        /// <summary>
        /// Create new role
        /// Access:
        /// Super Admin ONLY
        /// </summary>
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
        /// <summary>
        /// Update role details
        /// Access:
        /// Super Admin ONLY
        /// </summary>

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
        /// Activate / Deactivate role
        /// Access:
        /// Super Admin ONLY
        /// </summary>  

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
        /// <summary>
        /// Fetch role dropdown list
        /// Access:
        /// All authorized users
        /// </summary>

        // ========================
        // DROPDOWN
        // ========================

        // ========================
        // DROPDOWN
        // ========================

        public async Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(List<string> roles,string email,IDbTransaction? transaction = null)
        {
            var response = new APIGetResponseModel<List<DropdownModel>>();

            try
            {
                // Only these roles can access role management dropdown
                if (!(roles.Contains("Super Admin") ||
                      roles.Contains("Org Admin") ||
                      roles.Contains("Branch Admin")))
                {
                    response.IsSuccess = false;
                    response.ErrorMsgs.Add("Access denied.");
                    return response;
                }

                // Get all active roles from DAL
                response = await _dal.GetDropdown(email, transaction);

                if (response.Result == null)
                {
                    response.Result = new List<DropdownModel>();
                    response.TotalRecords = 0;
                    response.IsSuccess = true;

                    return response;
                }

                // =========================================
                // SUPER ADMIN
                // Can see all active roles
                // =========================================
                if (roles.Contains("Super Admin"))
                {
                    // No filtering required
                }

                // =========================================
                // ORG ADMIN
                // Cannot see Super Admin or Org Admin
                // =========================================
                else if (roles.Contains("Org Admin"))
                {
                    response.Result = response.Result.Where(x =>!string.Equals(x.Name,"Super Admin",StringComparison.OrdinalIgnoreCase)&&!string.Equals(x.Name,"Org Admin",StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // =========================================
                // BRANCH ADMIN
                // Cannot see Super Admin, Org Admin,
                // or Branch Admin
                // =========================================
                else if (roles.Contains("Branch Admin"))
                {
                    response.Result = response.Result.Where(x =>!string.Equals(x.Name,"Super Admin",StringComparison.OrdinalIgnoreCase)&&!string.Equals(x.Name,"Org Admin",StringComparison.OrdinalIgnoreCase)&&!string.Equals(x.Name,"Branch Admin",StringComparison.OrdinalIgnoreCase)).ToList();
                }

                response.TotalRecords = response.Result.Count;
                response.IsSuccess = true;
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