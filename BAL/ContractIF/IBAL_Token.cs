using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IBAL_Token
    {
        Task<APIGetResponseModel<List<TokenModel>>> GetAll(PaginationRequestDto request, List<string> roles, string? email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<TokenModel>> GetById(long id, List<string> roles, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<int>> GenerateToken(TokenRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<int>> ChangeStatus(TokenRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<TokenModel>> CallNextToken(TokenRequestDto request, List<string> roles, string email, IDbTransaction? transaction = null);
        Task<APIGetResponseModel<List<DropdownModel>>> GetDropdown(string email, IDbTransaction? transaction = null);
    }
}
