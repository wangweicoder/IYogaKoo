using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class tUserLoginInfoRepository : Repository<tUserLoginInfo>, ItUserLoginInfoRepository
    { 
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tUserLoginInfo> GetPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.LoginTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
         
        /// <summary>
        ///  根据Uid 获取Entity
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public tUserLoginInfo GetByUid(int Uid)
        {
            return dbSet.Where(a => a.Uid == Uid).FirstOrDefault();
        }

        public int updateEntity(tUserLoginInfo model)
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
    }
}
