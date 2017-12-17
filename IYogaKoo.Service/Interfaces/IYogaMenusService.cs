using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
namespace IYogaKoo.Service.Interfaces
{
    public  interface IYogaMenusService
    {
        int Add(ViewYogaMenus menus);
        List<ViewYogaMenus> GetMenusList();
        List<ViewYogaMenus> GetMenusList(int page, int pagesize, out int count);
        int Update(ViewYogaMenus model);

        ViewYogaMenus GetMenusByid(int id);

        int DelByid(int id);
    }
}
