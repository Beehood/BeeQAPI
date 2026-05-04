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
        Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<OrganizationModel>> GetById(long id, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(OrganizationRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(OrganizationRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, string userId, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null);
    }
}
