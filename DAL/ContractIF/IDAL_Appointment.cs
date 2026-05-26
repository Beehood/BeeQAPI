using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Appointment
    {
        Task<APIGetResponseModel<List<AppointmentModel>>> GetAll(AppointmentRequestDto request, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<AppointmentModel>> GetById(long id, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<int>> Insert(AppointmentRequestDto request, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<int>> Update(AppointmentRequestDto request, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<int>> ChangeStatus(long id, int status, string email, IDbTransaction? transaction = null);
    }
}
