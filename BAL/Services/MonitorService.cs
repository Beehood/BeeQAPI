using BAL.ContractIF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class MonitorService : IMonitorService
    {
        public async Task<string> GetBranchByKey(string monitorKey)
        {
            // TEMP
            return "1";

            // Later:
            // DB lookup → monitorKey → branchId
        }
    }
}
