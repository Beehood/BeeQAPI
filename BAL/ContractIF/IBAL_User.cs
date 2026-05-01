using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_User
    {
        Task<APIGetResponseModel<List<UserModel>>> GetAll(PaginationRequestDto request, TokenUserInfo user, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<UserModel>> GetById(long id, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Create(UserRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(UserRequestDto request, string userId, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, TokenUserInfo user, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(TokenUserInfo user, IDbTransaction? transaction = null);
    }
}

