using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_TimeSlot
    {
        // ========================
        // GET ALL (LIST)
        // ========================

        Task<APIGetResponseModel<List<TimeSlotModel>>> GetAll(PaginationRequestDto request, string email, IDbTransaction? transaction = null);

        // ========================
        // GET BY ID
        // ========================
        Task<APIGetResponseModel<TimeSlotModel>> GetById(long id,string email,IDbTransaction? transaction = null);

        // ========================
        // INSERT
        // ========================
        Task<APIGetResponseModel<int>> Insert(TimeSlotRequestDto request,string email,IDbTransaction? transaction = null);

        // ========================
        // UPDATE
        // ========================
        Task<APIGetResponseModel<int>> Update(TimeSlotRequestDto request,string email,IDbTransaction? transaction = null);

        // ========================
        // STATUS (TOGGLE)
        // ========================
        Task<APIGetResponseModel<int>> ChangeStatus(long id,string email,IDbTransaction? transaction = null);

        // ========================
        // DROPDOWN (for Appointment)
        // ========================
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(long serviceId,string email,IDbTransaction? transaction = null);
    }
}
