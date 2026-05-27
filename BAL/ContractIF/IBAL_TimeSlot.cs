using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_TimeSlot
    {
        Task<APIGetResponseModel<List<TimeSlotModel>>> GetAll(
      PaginationRequestDto request,
      List<string> roles,
      string? email,
      IDbTransaction? transaction = null);

        Task<APIGetResponseModel<TimeSlotModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(TimeSlotRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(TimeSlotRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id, List<string> roles, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(long serviceId, string email, IDbTransaction? transaction = null);
    }
}
