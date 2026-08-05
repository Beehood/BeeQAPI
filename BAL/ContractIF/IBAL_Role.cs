using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_Role
    {
        Task<APIGetResponseModel<List<RoleModel>>> GetAll(PaginationRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<RoleModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(RoleRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(RoleRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(List<string> roles,string email,IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdownByOrganization(long organizationId,string email,IDbTransaction? transaction = null);
    }
}
