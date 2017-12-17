using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        IQueryable<Order> Orders { get; }
        List<Order> GetOrdersByuid(int uid);

        List<Order> GetClassId(int UserId);

        int GetOrdersByclassid(int classid);
        int DeleteNO(int userId, int classId);
        List<Order> GetOrder(string whereStr, int page, int pagesize, out int count);

        int updateEntity(Order model);
    }
}
