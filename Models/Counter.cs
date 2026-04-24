using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CounterModel
    {
        public long CounterId { get; set; }

        // Reference to Branch
        public long BranchId { get; set; }

        public string CounterName { get; set; }

        public string? CounterCode { get; set; }


        // 1 = Active, 0 = Inactive
        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CounterRequestDto
    {
        public long CounterId { get; set; }

        // Required for create/update
        public long BranchId { get; set; }

        public string CounterName { get; set; }

        public string? CounterCode { get; set; }


        public int Status { get; set; }

        // Optional if using JWT, otherwise keep
        // public long UserId { get; set; }
    }
}
