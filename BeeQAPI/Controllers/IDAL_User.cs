using Models;
using System.Data;

namespace BeeQAPI.Controllers
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

