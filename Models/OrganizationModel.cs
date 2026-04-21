using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrganizationModel
    {
        public long? OrganizationId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? SubscriptionPlan { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class OrganizationRequestDto
    {
        public long? OrganizationId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? SubscriptionPlan { get; set; }
        public int? Status { get; set; }
        //public long? UserId { get; set; }
    }
   
    
       
    

}
