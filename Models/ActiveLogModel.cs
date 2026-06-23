using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ActivityLogModel
    {
        public long LogId { get; set; }

        public long? OrganizationId { get; set; }

        public long? BranchId { get; set; }

        public long? UserId { get; set; }

        public string? UserName { get; set; }

        public string? RoleName { get; set; }

        public string? ActionName { get; set; }

        public string? ModuleName { get; set; }

        public string? TableName { get; set; }

        public long? RecordId { get; set; }

        public string? Description { get; set; }

        public string? OldData { get; set; }

        public string? NewData { get; set; }

        public string? IpAddress { get; set; }

        public string? DeviceInfo { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class ActivityLogRequestDto
    {
        public long LogId { get; set; }

        public long? OrganizationId { get; set; }

        public long? BranchId { get; set; }

        public long? UserId { get; set; }

        public string? UserName { get; set; }

        public string? RoleName { get; set; }

        public string? ActionName { get; set; }

        public string? ModuleName { get; set; }

        public string? TableName { get; set; }

        public long? RecordId { get; set; }

        public string? Description { get; set; }

        public string? OldData { get; set; }

        public string? NewData { get; set; }

        public string? IpAddress { get; set; }

        public string? DeviceInfo { get; set; }
    }

   

}