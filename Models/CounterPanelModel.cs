using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CounterPanelDashboardModel
    {
        public CurrentServingModel? CurrentServing { get; set; }

        public List<WaitingQueueModel> WaitingQueue { get; set; }
            = new();

        public List<MissedQueueModel> MissedQueue { get; set; }
            = new();

        public CounterPanelStatsModel Stats { get; set; }
            = new();
    }

    public class CurrentServingModel
    {
        public long TokenId { get; set; }

        public string? TokenDisplay { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public string? ServiceName { get; set; }

        public long CounterId { get; set; }

        public string? CounterName { get; set; }

        public DateTime? CalledTime { get; set; }

        public DateTime? StartTime { get; set; }

        public int Status { get; set; }

        public string? StatusName { get; set; }
    }

    public class WaitingQueueModel
    {
        public long TokenId { get; set; }

        public string? TokenDisplay { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public string? ServiceName { get; set; }

        public int Priority { get; set; }

        public int WaitingMinutes { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class MissedQueueModel
    {
        public long TokenId { get; set; }

        public string? TokenDisplay { get; set; }

        public string? CustomerName { get; set; }

        public DateTime? CalledTime { get; set; }

        //public int RecallCount { get; set; }
    }

    public class CounterPanelStatsModel
    {
        public int WaitingCount { get; set; }

        public int ServingCount { get; set; }

        public int CompletedToday { get; set; }

        public int SkippedToday { get; set; }

        public decimal AvgWaitMinutes { get; set; }

        public decimal AvgServiceMinutes { get; set; }
    }

    public class CounterPanelActionRequestDto
    {
        public long CounterId { get; set; }

        public long TokenId { get; set; }
        public string? SearchKey { get; set; }

        public int PageNo { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Remarks { get; set; }
        public long ? BranchServiceId { get; set; }
    }

    public class CallNextTokenResponseDto
    {
        public long TokenId { get; set; }

        public string? TokenDisplay { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public string? ServiceName { get; set; }

        public long CounterId { get; set; }

        public DateTime? CalledTime { get; set; }
    }
}
