using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using System.Data.SqlClient;
using IYogaKoo.ViewModel.Commons.Helper;

namespace IYogaKoo.Dao
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public IQueryable<Order> Orders
        {
            get
            {
                return dbSet.AsNoTracking().Include("Class");
            }
        }


        public List<Order> GetOrdersByuid(int uid)
        {
            return dbSet.Include("Class").Where(c => c.UserId == uid && c.IsDeleted == false).OrderByDescending(c => c.CreateTime).ToList();
        }

        /// <summary>
        /// 前端专页调用查询 取classID
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<Order> GetClassId(int UserId)
        {
            return dbSet.Where(a => a.UserId == UserId && a.IsDeleted == false).ToList();
        }
        /// <summary>
        /// 取消兴趣
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteNO(int userId, int classId)
        {
            Order entity = dbSet.FirstOrDefault(i => i.UserId == userId && i.ClassId == classId);

            if (entity != null)
            {
                entity.IsDeleted = true;
                Context.Entry(entity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
                Update(entity);
                return Save();
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 判断活动是否已报名
        /// </summary>
        /// <param name="classid">活动编号</param>
        /// <returns></returns>
        public int GetOrdersByclassid(int classid)
        {
            return dbSet.Where(c => c.ClassId == classid && c.IsDeleted == false).Count();
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <returns></returns>
        public List<Order> GetOrder(string whereStr, int page, int pagesize, out int count)
        {
            whereStr = whereStr.TrimEnd(',');
            string[] whereArray = whereStr.Split(',');
            IQueryable<Order> linq = dbSet.OrderByDescending(a => a.CreateTime);

            if (!string.IsNullOrWhiteSpace(whereStr))
            {
                foreach (string item in whereArray)
                {
                    if (item.Contains("Phone"))
                    {
                        var value = item.Split('!').Last();
                        linq = linq.Where(p => p.Phone == value);
                    }
                    if (item.Contains("CreateTime"))
                    {
                        var value = Convert.ToDateTime(item.Split('!').Last());
                        linq = linq.Where(p => p.CreateTime >= value);
                    }
                    if (item.Contains("EndTime"))
                    {
                        var value = Convert.ToDateTime(item.Split('!').Last());
                        linq = linq.Where(p => p.CreateTime <= value);
                    }
                    if (item.Contains("UserId"))
                    {
                        var value = Convert.ToInt32(item.Split('!').Last());
                        linq = linq.Where(p => p.UserId == value);
                    }
                }
            }


            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();


            //string sql = " SELECT *  FROM [Order] order by CreateTime desc";
            //DataTable dt = SQLHelper.ExecuteDataTable(sql, null);
            //List<Order> orderList = DataTableHelper.TableToEntity<Order>(dt);
            //return orderList;
        }

        public int updateEntity(Order model)
        {

            var entity = dbSet.Find(model.Id);
            //var entity = dbSet.AsNoTracking().FirstOrDefault(p=>p.Id ==model.Id);

            if (entity != null)
            {
                Context.Entry(entity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            entity = model;

            Update(entity);

            return Save();


        }
    }
}
