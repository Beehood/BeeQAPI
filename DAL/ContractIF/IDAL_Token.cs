using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Token
    {
        Task<APIGetResponseModel<List<TokenModel>>> GetAll(PaginationRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<TokenModel>> GetById(long id, string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> GenerateToken( TokenRequestDto request, string email, IDbTransaction? transaction = null);

        Task<APIGetResponseModel<int>> ChangeStatus( TokenRequestDto request,string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<TokenModel>> CallNextToken( TokenRequestDto request, string email,IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<TokenStatusModel>>>GetStatuses(string email,IDbTransaction? transaction = null);

        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown( string email, IDbTransaction? transaction = null);
    }
}
