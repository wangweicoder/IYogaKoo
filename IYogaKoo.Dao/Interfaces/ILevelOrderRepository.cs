using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Dao.Interfaces
{
   public  interface ILevelOrderRepository:IRepository<LevelOrder>
    {
       List<LevelOrder> BackGetOrdersPageList(string Name, string OrderState, string TargetLevel, string OrderType,
         int page, int pagesize, out int count);
       List<LevelOrder> GetOrdersPageList(int page, int pagesize, string Ordertype, out int count); 
       LevelOrder GetUid(int UID);
       int updateEntity(LevelOrder model);
    }
}
