using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_Permission
    {
        Task<APIGetResponseModel<List<PermissionModel>>> GetAll(PaginationRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<PermissionModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(PermissionRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(PermissionRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null);
    }
}
