using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel; 

namespace IYogaKoo.Service.Interfaces
{
    public interface ILevelOrderService
    {
        int Add(ViewLevelOrder model);
        List<ViewLevelOrder> BackGetOrdersPageList(string Name, string OrderState, string TargetLevel, string OrderType,
         int page, int pagesize, out int count);
        List<ViewLevelOrder> GetOrdersPageList(int page, int pagesize, string Ordertype, out int count);

        ViewLevelOrder GetById(int id);
        ViewLevelOrder GetUid(int UID);
        int Update(ViewLevelOrder model);
        int Delete(string deletelist);
    }
}
