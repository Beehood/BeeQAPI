using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DisplayBoardModel
    {
        public long DisplayId { get; set; }
        public long BranchId { get; set; }
        public string DisplayName { get; set; }
        public string ScreenCode { get; set; }
        public int UpcomingLimit { get; set; }
        public int TemplateId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class DisplayBoardRequestDto
    {
        public string Action { get; set; }   // LIST, GETBYID, INSERT, UPDATE, STATUS, DROPDOWN
        public long DisplayId { get; set; }
        public long BranchId { get; set; }
        public string DisplayName { get; set; }
        public string ScreenCode { get; set; }
        public int UpcomingLimit { get; set; }
        public int TemplateId { get; set; }
        public bool Status { get; set; }
    }
}
