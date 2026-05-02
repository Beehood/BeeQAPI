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
        Task<APIGetResponseModel<List<OrganizationModel>>> GetAll(PaginationRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<OrganizationModel>> GetById(long id, List<string> roles, string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(OrganizationRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(OrganizationRequestDto request, List<string> roles, string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email,IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(IDbTransaction? transaction = null);
    }
}
