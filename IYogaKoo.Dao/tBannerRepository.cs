using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class tBannerRepository : Repository<tBanner>, ItBannerRepository
    {
        /// <summary>
        /// 0 首页Banner 1 活动回顾Banner  2 活动预告Banner
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public List<tBanner> GettBannerList(int iType)
        { 
            return dbSet.Where(a=>a.iType==iType).Take(5).OrderByDescending(a => a.CreateDate).ToList();
        }
        /// <summary>
        /// 导师列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tBanner> GettBannerPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 升级导师列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tBanner> GettBannerPageListUp(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<tBanner> GettBannerPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        { 
            IQueryable<tBanner> linq = dbSet.OrderBy(a => a.CreateDate);

           
            count = linq.Count();// dbSet.Count();
             
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 根据Uid 获取list列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tBanner> GettBannerUid(int id)
        {
            return dbSet.Where(a => a.ID == id).ToList();
        }
        /// <summary>
        /// 根据Uid 获取Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tBanner GettBannerById(int id)
        {
            return dbSet.Where(a => a.ID == id).FirstOrDefault();
        }


        public int updateEntity(tBanner model)
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
