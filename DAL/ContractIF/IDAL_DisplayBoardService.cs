using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_DisplayBoardService
    {
        // LIST services mapped to a display
        Task<APIGetResponseModel<List<DisplayBoardServiceModel>>> GetAll(long displayId,string email,IDbTransaction? transaction = null);

        // INSERT (map service to display)
        Task<APIGetResponseModel<int>> Insert(DisplayBoardServiceRequestDto request,string email,IDbTransaction? transaction = null);

        // DELETE (remove mapping)
        Task<APIGetResponseModel<int>> Delete(long id,string email,IDbTransaction? transaction = null);
    }

}
