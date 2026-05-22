using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_DisplayBoard
    {
        Task<APIGetResponseModel<List<DisplayBoardModel>>> GetAll(PaginationRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<DisplayBoardModel>> GetById(long id, string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Insert(DisplayBoardRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(DisplayBoardRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus( long id,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null);
        Task<List<QueueDisplayModel>> GetDisplayData(string screenCode);
    }
}
