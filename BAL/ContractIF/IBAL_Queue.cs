using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_Queue
    {
        Task<APIGetResponseModel<List<QueueModel>>> GetAll(PaginationRequestDto request, List<string> roles,string? email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<QueueModel>> GetById(long tokenId,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(QueueRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update( QueueRequestDto request, List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(QueueRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<QueueDisplayModel>>> GetQueueDisplay(string branchId);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null);
    }
}

