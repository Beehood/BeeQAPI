using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
 
        public class DeviceModel
        {
            public long DeviceId { get; set; }
            public long OrganizationId { get; set; }
            public long BranchId { get; set; }
            public string? DeviceType { get; set; }
            public string? DeviceName { get; set; }
            public string? IpAddress { get; set; }
            public string? MacAddress { get; set; }
            public bool IsOnline { get; set; }
            public DateTime? LastPing { get; set; }
            public string? Settings { get; set; }
            public bool Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public class DeviceRequestDto   
        {
            public long DeviceId { get; set; }
            public long OrganizationId { get; set; }
            public long BranchId { get; set; }
            public string? DeviceType { get; set; }
            public string? DeviceName { get; set; }
            public string? IpAddress { get; set; }
            public string? MacAddress { get; set; }
            public bool Status { get; set; }
        }

        public class DeviceStatusRequestDto
        {
            public long DeviceId { get; set; }
        }
    }