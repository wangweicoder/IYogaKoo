using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
namespace IYogaKoo.Service
{
    public class YogaMenusServiceImpl : IYogaMenusService
    {
        IYogaMenusRepository Respository;
        public YogaMenusServiceImpl(IYogaMenusRepository Respository)
        {
            this.Respository = Respository;
        }
        public List<ViewYogaMenus> GetMenusList()
        {
            List<YogaMenus> list = Respository.GetYogaMenusLists();
            List<ViewYogaMenus> model = new List<ViewYogaMenus>();
            foreach (var item in list)
            {
                model.Add(ViewYogaMenus.ToViewModel(item));
            }
            return model;
        }

        public int Add(ViewYogaMenus menus)
        {
            Respository.Add(ViewYogaMenus.ToEntity(menus));
            return menus.Id;
        }
        public List<ViewYogaMenus> GetMenusList(int page, int pagesize, out int count)
        {
            List<YogaMenus> list = Respository.GetYogaMenusList(page, pagesize, out  count);

            List<ViewYogaMenus> model = new List<ViewYogaMenus>();

            foreach (var item in list)
            {
                model.Add(ViewYogaMenus.ToViewModel(item));
            }
            return model;
        }

        public int Update(ViewYogaMenus model)
        {
            Respository.updateEntity(ViewYogaMenus.ToEntity(model));
            return Respository.Save();
        }


        public ViewYogaMenus GetMenusByid(int id)
        {
            return ViewYogaMenus.ToViewModel(Respository.GetMenusByid(id));
        }

        public int DelByid(int id)
        {
            Respository.Delete(Respository.Get(id));
            return Respository.Save();
        }
    }
}
