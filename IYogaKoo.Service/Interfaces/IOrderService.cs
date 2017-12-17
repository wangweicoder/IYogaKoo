using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;
using System.Linq.Expressions;

namespace IYogaKoo.Service.Interfaces
{
    public interface IOrderService
    {
        ViewOrder Get(int classID, int userID);

        ViewOrder Get(int id);

        void Edit(ViewOrder vo);

        Result Add(ViewOrder order);

        PageResult<ViewOrder> GetByUser(int userID,int page,int size);

        PageResult<ViewOrder> GetByClass(int classID, int page, int size);

        Result Pay(ViewOrder vo);

        int AppliedNumber(int classID,int timeout);

        Result CanApply(int classID,int count);

        List<ViewOrder> GetOrdersByuid(int uid);

        List<ViewOrder> GetClassId(int UserId);
        int DeleteNO(int userId, int classId);
        int GetOrdersByclassid(int classid);

        List<ViewOrder> GetOrder(string whereStr, int page, int pagesize, out int count);

        int updateEntity(ViewOrder model);
    }
}
