using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReportModel
    {
        // Common
        public string? ReportDate { get; set; }

        // Token Summary
        public int TotalTokens { get; set; }
        public int WaitingTokens { get; set; }
        public int CalledTokens { get; set; }
        public int ServingTokens { get; set; }
        public int CompletedTokens { get; set; }
        public int MissedTokens { get; set; }
        public int CancelledTokens { get; set; }

        // Service Performance
        public string? ServiceName { get; set; }

        // Counter Performance
        public string? CounterName { get; set; }
    }

    public class ReportRequestDto
    {
        public string? Action { get; set; }

        public long? OrganizationId { get; set; }

        public long? BranchId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}