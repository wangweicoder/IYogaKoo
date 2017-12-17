using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao
{
    public class LevelOrderRepository : Repository<LevelOrder>, ILevelOrderRepository
    { 
        public List<LevelOrder> GetOrdersPageList(int page, int pagesize, string Ordertype, out int count)
        {
            IQueryable<LevelOrder> los = dbSet.Where(a => a.OrderDel == 0).OrderByDescending(a=>a.CreateTime);
            count = los.Count();
            return los.ToList();  
        }
        /// <summary>
        /// 根据UID取最近一条数据
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public LevelOrder GetUid(int UID)
        {
            return dbSet.Where(a => a.UID == UID).OrderByDescending(a => a.CreateTime).FirstOrDefault();
        }
        public int updateEntity(LevelOrder model)
        {
            var entity = dbSet.Find(model.ID);
            
            if (entity != null)
            {
                Context.Entry(entity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            entity = model;

            Update(entity);

            return Save();


        }

       /// <summary>
       /// 后台升级导师查询方法
       /// </summary>
       /// <param name="Name"></param>
       /// <param name="OrderState"></param>
       /// <param name="TargetLevel"></param>
       /// <param name="OrderType"></param>
       /// <param name="page"></param>
       /// <param name="pagesize"></param>
       /// <param name="count"></param>
       /// <returns></returns>
        public List<LevelOrder> BackGetOrdersPageList(string Name, string OrderState, string TargetLevel, string OrderType, 
            int page, int pagesize, out int count)
        {
            IQueryable<LevelOrder> los = dbSet.Where(a => a.OrderDel == 0);
            if (!string.IsNullOrEmpty(Name)) {
                los = los.Where(a => a.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(OrderState))
            {
                los = los.Where(a => a.OrderState.Contains(OrderState));
            }
            if (!string.IsNullOrEmpty(TargetLevel))
            {
                los = los.Where(a => a.TargetLevel.Contains(TargetLevel));
            }
            if (!string.IsNullOrEmpty(OrderType))
            {
                los = los.Where(a => a.OrderType.Contains(OrderType));
            }
            count = los.Count();
            return los.OrderByDescending(a => a.CreateTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

    }
}
