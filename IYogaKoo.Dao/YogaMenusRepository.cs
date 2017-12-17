using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao
{
    public class YogaMenusRepository:Repository<YogaMenus>,IYogaMenusRepository
    {

        public List<YogaMenus> GetYogaMenusLists()
        {
            return dbSet.ToList();
        }
         

        public List<YogaMenus> GetYogaMenusList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderBy(a=>a.ordervalue).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }


        public  int updateEntity(YogaMenus menus)
        {
            var entity = dbSet.Find(menus.Id);
            if (entity != null)
            {
                Context.Entry(entity).State = System.Data.EntityState.Detached;
            }
            entity = menus;
            Update(entity);
            return Save();
        } 

        public YogaMenus GetMenusByid(int id)
        {
          return  dbSet.Where(a => a.Id == id).FirstOrDefault() ;
        }


      
    }
}
