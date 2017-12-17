using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class YogiProfileRepository : Repository<YogiProfile>, IYogiProfileRepository
    {
        public List<YogiProfile> GetYogiProfileList()
        { 
            return dbSet.OrderByDescending(a => a.CreateDate).ToList();
        }
        public List<YogiProfile> GetYogiProfilePageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<YogiProfile> GetYogiProfilePageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            IQueryable<YogiProfile> linq = dbSet.OrderBy(a => a.CreateDate);

            
            count = linq.Count();// dbSet.Count();
             
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 根据Uid 获取YogiProfile列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogiProfile> GetYogiProfileUid(int id)
        {
            return dbSet.Where(a => a.UID == id).ToList();
        }
        /// <summary>
        /// 根据Uid 获取YogiProfile 信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogiProfile GetYogiProfileById(int id)
        {
            return dbSet.Where(a => a.UID == id).FirstOrDefault();
        }


        public int updateEntity(YogiProfile model)
        { 
            var entity = dbSet.Find(model.ProfileID);
            
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
