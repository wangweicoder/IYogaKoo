using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
using IYogaKoo.Dao.Interfaces;
namespace IYogaKoo.Dao
{
    public  class CenterStareRepository:Repository<CenterStare>,ICenterStareRepository
    {
        public List<CenterStare> GetCentersPageList(int mid, out int count)
        { 
            IQueryable<CenterStare> list=dbSet.Where(a => a.Mid == mid && a.Satate == 0);
            count = list.Count();
            return list.ToList(); 
        } 

        public int IfUidSave(int uid, int mid)
        {
            return dbSet.Where(a => a.Mid == mid && a.Uid == uid).Count(); 
        }
 
    }
}
