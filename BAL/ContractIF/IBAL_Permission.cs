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
        Task<APIGetResponseModel<List<PermissionModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<long>> Create(PermissionRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<long>> Update(PermissionRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, TokenUserInfo user, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(TokenUserInfo user, IDbTransaction? transaction = null);
    }
}
