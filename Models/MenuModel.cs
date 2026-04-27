using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MenuModel
    {
        public long Menu_Id { get; set; }
        public string Name { get; set; } = "";
        public string Url { get; set; }= "";
        public string Icon { get; set; } = "";
        public long? Parent_Id { get; set; }
        public int Order_No { get; set; }
    }
}
