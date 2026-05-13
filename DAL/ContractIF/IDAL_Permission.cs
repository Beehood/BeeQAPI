using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Permission
    {
        Task<APIGetResponseModel<List<PermissionModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<PermissionModel>> GetById(long id, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(PermissionRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(PermissionRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null);
    }
}
