using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_BranchDevice
    {
        Task<APIGetResponseModel<List<DeviceModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<DeviceModel>> GetById(long id, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(DeviceRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(DeviceRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null);
    }
}
