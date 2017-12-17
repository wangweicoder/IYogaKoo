using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
namespace IYogaKoo.Client
{
    public   class YogaMenusServiceClient:IYogaMenusService,IDisposable
    {

        public IYogaMenusService YogaMenusServiceImpl { get; set; }
        public YogaMenusServiceClient()
        {
            YogaMenusServiceImpl = new YogaMenusServiceImpl(new YogaMenusRepository());
        }
        public void Dispose()
        {
          
        }


        public List<ViewYogaMenus> GetMenusList()
        {
            try
            {
                return YogaMenusServiceImpl.GetMenusList();
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }
        public List<ViewYogaMenus> GetMenusList(int page, int pagesize, out int count)
        {
            try
            {
                return YogaMenusServiceImpl.GetMenusList(page, pagesize, out  count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int Add(ViewYogaMenus menus)
        {
            return YogaMenusServiceImpl.Add(menus);
        }

        public int Update(ViewYogaMenus model)
        {
            try
            {
                return YogaMenusServiceImpl.Update(model);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }


        public ViewYogaMenus GetMenusByid(int id)
        {
            try
            {
                return YogaMenusServiceImpl.GetMenusByid(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        public int DelByid(int id)
        {
            try
            {
                return YogaMenusServiceImpl.DelByid(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
