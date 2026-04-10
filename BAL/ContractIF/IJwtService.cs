using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(TokenUserInfo user);
    }
}
