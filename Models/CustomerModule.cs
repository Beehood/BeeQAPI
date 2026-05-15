using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CustomerModel
    {
        public long CustomerId { get; set; }

        public long OrganizationId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string? Email { get; set; }

        public bool IsVip { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CustomerRequestDto
    {
        public long CustomerId { get; set; }

        public long OrganizationId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string? Email { get; set; }

        public bool IsVip { get; set; }

        public bool Status { get; set; }
    }
}
