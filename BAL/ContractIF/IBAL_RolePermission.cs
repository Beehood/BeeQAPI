using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_RolePermission
    {
        Task<APIGetResponseModel<List<RolePermissionModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<RolePermissionModel>>> GetByRoleId( long roleId,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(RolePermissionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> BulkAssign(RolePermissionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Delete(RolePermissionRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);
    }
}

