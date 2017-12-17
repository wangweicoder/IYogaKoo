using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
namespace IYogaKoo.Dao.Interfaces
{
    public  interface IYogaMenusRepository:IRepository<YogaMenus>
    { 
        List<YogaMenus> GetYogaMenusLists();
        List<YogaMenus> GetYogaMenusList(int page, int pagesize, out int count);
       
        int updateEntity(YogaMenus menus);

        YogaMenus GetMenusByid(int id);
 
    }
}
