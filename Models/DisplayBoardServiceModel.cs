using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DisplayBoardServiceModel
   {
        public long Id { get; set; }
      public long DisplayId { get; set; }
        public long BranchServiceId { get; set; }
   }



       public class DisplayBoardServiceRequestDto
       {
            public string Action { get; set; }   // INSERT, LIST, DELETE
           public long Id { get; set; }
            public long DisplayId { get; set; }
            public long BranchServiceId { get; set; }
        }
}
