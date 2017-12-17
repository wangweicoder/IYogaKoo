using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class FollowRepository : Repository<Follow>, IFollowRepository
    {
        /// <summary>
        /// 获取谁关注了我的loginType信息状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginType">0，1 没有查看 ；2 已经查看完</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Follow> GetFollowQuiltUidList(int QuiltUid, int loginType, out int count)
        {
            IQueryable<Follow> linq = dbSet.Where(a => a.QuiltUid == QuiltUid && a.isfollow == true && a.loginType == loginType);
            count = linq.Count();
            return linq.ToList();
        }
        
        public List<Follow> GetFollowPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.FollowDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<Follow> GetFollowPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            IQueryable<Follow> linq = dbSet.OrderBy(a => a.FollowDate);

            count = linq.Count();// dbSet.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 获得自己关注的人
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<Follow> GetFollowQuiltUidList(int id)
        {
            return dbSet.Where(a => a.Uid == id && a.isfollow == true).ToList();
        }
        /// <summary>
        /// 瑜伽圈
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns>1120</returns>
        public List<Follow> GetFollowUidQuiltList(int uid)
        {
            return dbSet.Where(a => a.Uid == uid && a.isfollow == true ||a.QuiltUid == uid).ToList();
        
        }
        /// <summary>
        /// 粉丝列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<Follow> GetFollowQuiltUidList(int id, int page, int pagesize, out int count)
        {
            IQueryable<Follow> Iqueryf = dbSet.Where(a => a.QuiltUid == id && a.isfollow == true).OrderByDescending(a => a.FollowDate);
            count = Iqueryf.Count();
            return Iqueryf.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 粉丝列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<Follow> GetFollowQuiltUidList(int itype,int id, int page, int pagesize, out int count)
        {
            IQueryable<Follow> Iqueryf = dbSet.Where(a => a.QuiltUid == id && a.isfollow == true&&a.QuiltCenterID==itype).OrderByDescending(a => a.FollowDate);
            count = Iqueryf.Count();
            return Iqueryf.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<Follow> GetFollowUidList(int id, int page, int pagesize, out int count)
        {
            IQueryable<Follow> Iqueryf = dbSet.Where(a => a.Uid == id && a.isfollow == true).OrderByDescending(a => a.FollowDate);
            count = Iqueryf.Count();            
            return Iqueryf.Skip((page-1)*pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 分类的关注列表
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<Follow> GetFollowUidList(int itype,int id, int page, int pagesize, out int count)
        {
            IQueryable<Follow> Iqueryf = dbSet.Where(a => a.Uid == id && a.isfollow == true &&a.iType==itype).OrderByDescending(a => a.FollowDate);
            count = Iqueryf.Count();
            return Iqueryf.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public Follow GetFollowById(int id)
        {
            return dbSet.Where(a => a.QuiltUid == id && a.isfollow == true).FirstOrDefault();
        }
        public List<Follow> GetFollowByQuiltUid(int id)
        {
            return dbSet.Where(a => a.QuiltUid == id && a.isfollow == true).ToList();
        }
        public int GetFollowByCount(int id)
        {
            return dbSet.Where(a => a.QuiltUid == id && a.isfollow == true).Count();
        }
        public int GetFollowByUid(int id)
        {
            int icount= dbSet.Where(a => a.Uid == id && a.isfollow==true).Count();
            return icount;
        }
        /// <summary>
        /// 根据uid 及被关注uid 判断是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="QuiltUid"></param>
        /// <returns></returns>
        public Follow GetFollowById(int uid, int QuiltUid)
        {
            return dbSet.Where(a => a.Uid == uid && a.QuiltUid == QuiltUid).FirstOrDefault();
        }
        public int updateEntity(Follow model)
        {

            var entity = dbSet.Find(model.Followid);

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
