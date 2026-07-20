using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ServiceModel
    {
        public long ServiceId { get; set; }
        public long OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public long BranchId { get; set; }
        public string? BranchName { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceCode { get; set; }
        public int? EstimatedTime { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ServiceRequestDto
    {
        public long ServiceId { get; set; }
        public long OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public long BranchId { get; set; }
        public string? BranchName { get; set; }
        public long BranchServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceCode { get; set; }
        public int? EstimatedTime { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public string? SearchKey { get; set; }
        public int? PageNo { get; set; }
        public long? UserId { get; set; }
      
    }
    //public class BranchServiceSearchKeys
    //{
    //    public int branch_service_id { get; set; }
    //    public string? SearchKey { get; set; }
    //    public int PageNo { get; set; } = 1;
    //    public int PageSize { get; set; } = 10;
    //}
    public class ServiceStatusRequestDto
    {
        public long ServiceId { get; set; }
    }
}
