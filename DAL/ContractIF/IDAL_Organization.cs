using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Organization
    {
        Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(
            PaginationRequestDto request,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<OrganizationModel>> GetById(
            long id,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Insert(
            OrganizationRequestDto request,
            string userId,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> Update(
            OrganizationRequestDto request,
            string userId,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<long>> ChangeStatus(
            long id,
            int status,
            long userId,
            IDbTransaction? transaction = null);
    }
}
