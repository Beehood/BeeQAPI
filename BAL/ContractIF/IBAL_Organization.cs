using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IOrganizationBAL
    {
        Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(
            PaginationRequestDto request,
            IDbTransaction? transaction = null
        );

        Task<APIGetResponseModel<OrganizationModel>> GetById(
            long id,
            IDbTransaction? transaction = null
        );

        Task<APIGetResponseModel<long>> Create(
            OrganizationRequestDto request,
            IDbTransaction? transaction = null
        );

        Task<APIGetResponseModel<long>> Update(
            OrganizationRequestDto request,
            IDbTransaction? transaction = null
        );

        Task<APIGetResponseModel<long>> ChangeStatus(
            long id,
            int status,
            long userId,
            IDbTransaction? transaction = null
        );
    }
}
