using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ServiceModel
    {
        public int Service_id { get; set; }

        public string OrganizationId { get; set; } = "";
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceCode { get; set; } = ""; 
        public string EstimatedTime { get; set; } = "";
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime?CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ServiceSearchKeys
    {
        public int Id { get; set; }
        public string? SearchKey { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
    public class BranchServiceModel
    {
        public int branch_service_id { get; set; }
        public int branch_id { get; set; }
        public int service_id { get; set; }

        public string prefix { get; set; }
        public int daily_limit { get; set; }

        public int status { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

        public string? created_by { get; set; }
        public string? updated_by { get; set; }
    }

    public class BranchServiceSearchKeys
    {
        public int branch_service_id { get; set; }
        public string? SearchKey { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
