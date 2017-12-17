using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao
{
    public class InterestRepository : Repository<InterestedClass>, IInterestRepository
    {
        public IQueryable<InterestedClass> Interests()
        {
            dbSet.Include("Class");
            return dbSet;
        }

        public int Delete(int classId, int userId)
        {
            var entity = dbSet.FirstOrDefault(i => i.UserId == userId && i.ClassId == classId);
            dbSet.Remove(entity);
            int rel = Save();
            return rel;
        }
        public int Count(int classId)
        {
            return dbSet.Where(x => x.ClassId == classId).Count();
        }

        public bool Exists(int classId, int userId)
        {
            IQueryable<InterestedClass> iquery = dbSet.Where(i => i.ClassId == classId && i.UserId == userId);
            if (iquery.Count() > 0)
                return true;
            else
                return false;
        }

        public List<InterestedClass> GetListClassId(int ClassId)
        {
            return dbSet.Where(a => a.ClassId == ClassId).ToList();
        }

        /// <summary>
        /// 我感兴趣的活动
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        /// a.IsDeleted ==true代表已删除
        public List<InterestedClass> GetListClassByUId(int uid)
        {
            return dbSet.Include("Class").Where(a => a.UserId == uid && a.IsDeleted == false).OrderByDescending(a => a.CreateTime).ToList();
        }

        /// <summary>
        /// 前端专页调用查询 取classID
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<InterestedClass> GetClassId(int UserId)
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
            InterestedClass entity = dbSet.FirstOrDefault(i => i.UserId == userId && i.ClassId == classId);

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

    }
}
