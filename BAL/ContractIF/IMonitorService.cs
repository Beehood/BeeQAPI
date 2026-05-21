using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ContractIF
{
    public interface IMonitorService
    {
        Task<string> GetBranchByKey(string monitorKey);
    }
}
