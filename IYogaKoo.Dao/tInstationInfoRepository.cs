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
    /// 站内信
    /// </summary>
    public class tInstationInfoRepository : Repository<tInstationInfo>, ItInstationInfoRepository
    {
        /// <summary>
        /// 查询状态信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="loginType"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tInstationInfo> GetPageListWhereUidAndloginType(int uid,int loginType,out int count)
        {
            IQueryable<tInstationInfo> linq = dbSet.Where(a => a.Uid == uid && a.loginType == loginType && a.ifDel==false);

            count = linq.Count();

            return linq.OrderByDescending(a => a.CreateTime).ToList();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tInstationInfo> GetPageList(int uid, int page, int pagesize, out int count)
        {
            IQueryable<tInstationInfo> linq = dbSet.Where(a => a.Uid == uid && a.ifDel == false);
            count = linq.Count();

            return linq.OrderByDescending(a => a.CreateTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 后台列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tInstationInfo> GetPageList(string content, int iType, DateTime? CreateTime, int page, int pagesize, out int count)
        {
            IQueryable<tInstationInfo> linq = dbSet.OrderBy(a => a.CreateTime);
            if (!string.IsNullOrEmpty(content))
            {
                linq = linq.Where(a => a.sContent.Contains(content));
            }
            if (iType != 0)
            {
                linq = linq.Where(a => a.iType == iType);
            }
            if (CreateTime != null)
            {
                linq = linq.Where(a => a.CreateTime == CreateTime);
            }
            count = linq.Count();

            return linq.OrderByDescending(a => a.CreateTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        ///  根据Uid 获取Entity
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public tInstationInfo GetByUid(int Uid)
        {
            return dbSet.Where(a => a.Uid == Uid).FirstOrDefault();
        }
        /// <summary>
        /// 根据内容获取所有UId
        /// </summary>
        /// <param name="sContent"></param>
        /// <returns></returns>
        public List<tInstationInfo> GetByContent(string sContent)
        {
            return dbSet.Where(a => a.sContent == sContent).ToList();
        }

        public int updateEntity(tInstationInfo model)
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
