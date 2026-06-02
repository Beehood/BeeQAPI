using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Threading.Tasks;

namespace BAL.ContractIF
{

    public interface IBAL_DisplayBoard
    {
        Task<APIGetResponseModel<List<DisplayBoardModel>>> GetAll(PaginationRequestDto request,List<string> roles,string? email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<DisplayBoardModel>> GetById(long id, List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Create(DisplayBoardRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> Update(DisplayBoardRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus(long id,List<string> roles,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email,IDbTransaction? transaction = null);
        Task<List<QueueDisplayModel>> GetDisplayData(string username);
    }
}

