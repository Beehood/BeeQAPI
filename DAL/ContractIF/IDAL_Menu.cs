using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContractIF
{
    public interface IDAL_Menu
    {
        Task<List<MenuModel>> GetSidebar(long userId);
    }
}
