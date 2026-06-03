using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_DisplayBoardService
    {
        // GET all services mapped to a display
        Task<APIGetResponseModel<List<DisplayBoardServiceModel>>> GetAll(long displayId,List<string> roles,string? email,IDbTransaction? transaction = null);

        // CREATE (map service to display)
        Task<APIGetResponseModel<int>> Create(DisplayBoardServiceRequestDto request,List<string> roles,string email,IDbTransaction? transaction = null);

        // DELETE (remove mapping)
        Task<APIGetResponseModel<int>> Delete(long id,List<string> roles,string email,IDbTransaction? transaction = null);
    }
}
