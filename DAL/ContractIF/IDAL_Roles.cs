using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Role
    {
        Task<APIGetResponseModel<List<RoleModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<RoleModel>> GetById(long id, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(RoleRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(RoleRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdownByOrganization(
    long organizationId,
    string email,
    IDbTransaction? transaction = null);
    }

}


