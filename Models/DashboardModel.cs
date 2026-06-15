using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DashboardModel
    {
        public DashboardSummaryModel Summary { get; set; } = new();

        public DashboardQueueModel QueueStats { get; set; } = new();

        public List<DashboardTrendModel> QueueTrend { get; set; } = new();

        public List<DashboardBranchModel> TopBranches { get; set; } = new();

        public List<DashboardServiceModel> TopServices { get; set; } = new();

        public List<DashboardActivityModel> RecentActivities { get; set; } = new();
    }
    public class DashboardSummaryModel
    {
        public int OrganizationCount { get; set; }

        public int BranchCount { get; set; }

        public int UserCount { get; set; }

        public int CounterCount { get; set; }

        public int ServiceCount { get; set; }

        public int AppointmentCount { get; set; }
    }
    public class DashboardQueueModel
    {
        public int WaitingCount { get; set; }

        public int ServingCount { get; set; }

        public int CompletedCount { get; set; }

        public int MissedCount { get; set; }

        public decimal AvgWaitMinutes { get; set; }

        public decimal AvgServiceMinutes { get; set; }

        public int ActiveCounters { get; set; }

        public int CompletedToday { get; set; }
    }
    public class DashboardTrendModel
    {
        public string? TrendDate { get; set; }

        public int GeneratedTokens { get; set; }

        public int CompletedTokens { get; set; }
    }
    public class DashboardBranchModel
    {
        public string? BranchName { get; set; }

        public int TokenCount { get; set; }
    }

    public class DashboardServiceModel
    {
        public string? ServiceName { get; set; }

        public int TokenCount { get; set; }
    }
    public class DashboardActivityModel
    {
        public string? ActivityText { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
    public class DashboardRequestDto
    {
        public long? OrganizationId { get; set; }
    }

}
