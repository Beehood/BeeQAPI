using Models;
using System.Data;

namespace BAL.ContractIF
{

    namespace BAL.ContractIF
    {
        public interface IBAL_Service
        {
            Task<APIGetResponseModel<List<ServiceModel>>> GetAll(PaginationRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

            Task<APIGetResponseModel<ServiceModel>> GetById(long id,List<string> roles,string email,IDbTransaction? transaction = null);

            Task<APIGetResponseModel<int>> Create(ServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

            Task<APIGetResponseModel<int>> Update(ServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

            Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null);

            Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null);
        }
    }
}
