using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_Organization
    {
        Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(
            PaginationRequestDto request,
            TokenUserInfo user,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<OrganizationModel>> GetById(
            long id,
            TokenUserInfo user,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Create(
            OrganizationRequestDto request,
            string userId,
            TokenUserInfo user,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(
            OrganizationRequestDto request,
            string userId,
            TokenUserInfo user,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(
            long id,
            int status,
            long userId,
            TokenUserInfo user,
            IDbTransaction? transaction = null);
    }
}
