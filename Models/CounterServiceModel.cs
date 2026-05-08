using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CounterServiceModel
    {
        public long CounterServiceId { get; set; }

        public long CounterId { get; set; }

        public long ServiceId { get; set; }

        // Dropdown display values
        public string? CounterName { get; set; }

        public string? ServiceName { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CounterServiceRequestDto
    {
        public long CounterServiceId { get; set; }

        public long CounterId { get; set; }

        public long ServiceId { get; set; }

        public bool? Status { get; set; }

        public long? UserId { get; set; }
    }
}
