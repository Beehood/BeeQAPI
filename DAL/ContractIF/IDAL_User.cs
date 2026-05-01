using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_User
    {
        Task<APIGetResponseModel<List<UserModel>>> GetAll(PaginationRequestDto request, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<UserModel>> GetById(long id, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Insert(UserRequestDto request, string userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(UserRequestDto request, string userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(long id, int status, long userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null);
    }
}
