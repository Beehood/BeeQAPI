using Microsoft.AspNetCore.SignalR;

namespace BeeQAPI.Realtime
{
    public class QueueHub : Hub
    {
        public async Task JoinBranch(string branchId)
        {
            //  ADD THIS VALIDATION
            if (string.IsNullOrWhiteSpace(branchId))
            {
                throw new Exception("BranchId is required ❌");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, branchId);
        }
    }
}