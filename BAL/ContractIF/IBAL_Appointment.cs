using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{ 
        public interface IBAL_Appointment
        {
            Task<APIGetResponseModel<List<AppointmentModel>>> GetAll(AppointmentRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null);
            Task<APIGetResponseModel<AppointmentModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null);
            Task<APIGetResponseModel<int>> Create(AppointmentRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);
            Task<APIGetResponseModel<int>> Update(AppointmentRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);
            Task<APIGetResponseModel<int>> ChangeStatus(long id, int status, List<string> roles, string email, IDbTransaction? transaction = null);
        }
}