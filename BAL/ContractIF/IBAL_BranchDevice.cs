using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_BranchDevice
    {
        Task<APIGetResponseModel<List<DeviceModel>>> GetAll(PaginationRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<DeviceModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(DeviceRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(DeviceRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null);
    }
}