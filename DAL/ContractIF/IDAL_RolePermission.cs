using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_RolePermission
    {
        Task<APIGetResponseModel<List<RolePermissionModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<RolePermissionModel>>> GetByRoleId(long roleId, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(RolePermissionRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> BulkInsert(RolePermissionRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Delete(RolePermissionRequestDto request, string email, IDbTransaction? transaction = null);
    }
}
