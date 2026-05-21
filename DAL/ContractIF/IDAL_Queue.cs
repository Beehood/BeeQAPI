using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Queue
    {
        Task<APIGetResponseModel<List<QueueModel>>> GetAll(
            PaginationRequestDto request,
            string email,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<QueueModel>> GetById(
            long tokenId,
            string email,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(
            QueueRequestDto request,
            string email,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(
            QueueRequestDto request,
            string email,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(
            QueueRequestDto request,
            string email,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<QueueDisplayModel>>> GetQueueDisplay(
            string email,
            IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(
            string email,
            IDbTransaction? transaction = null);
    }
}