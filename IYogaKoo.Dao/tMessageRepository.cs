using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class tMessageRepository : Repository<tMessage>, ItMessageRepository
    {
       
        /// <summary>
        /// 谁评论了我
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ParentID">0</param>
        /// <returns></returns>
        public List<tMessage> GetByMessage(int toType, int id, int ParentID)
        {
            IQueryable<tMessage> linq = dbSet.OrderBy(a=>a.CreateDate);
            if (toType != 0)
            {
                if (toType == 1)
                {
                    linq = linq.Where(a =>  a.ToUid == id && a.ParentID == ParentID && (a.ToType == 1 || a.ToType == 0));
                }
                else
                {
                    linq = linq.Where(a => a.ToType == toType && a.ToUid == id && a.ParentID == ParentID);
                }
            }
            else
            {
                linq = linq.Where(a => a.ToUid == id && a.ParentID == ParentID);
            }
            return linq.OrderByDescending(a=>a.CreateDate).ToList();
        }
        public List<tMessage> GetPageListWhereUidAndloginType(int uid, int loginType, out int count)
        {
            IQueryable<tMessage> linq = dbSet.Where(a => a.ToUid == uid && a.loginType == loginType);
            count = linq.Count();
            return linq.ToList();
        }
       
        /// <summary>
        /// 我评论了谁
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public List<tMessage> GetByMessageFromUid(int toType, int id, int ParentID)
        {
            IQueryable<tMessage> linq = dbSet.OrderBy(a => a.CreateDate);
            if (toType != 0)
            {
               // linq = linq.Where(a => a.ToType == toType && a.FromUid == id && a.ParentID == ParentID);
                if (toType == 1)
                {
                    linq = linq.Where(a => a.FromUid == id && a.ParentID == ParentID && (a.ToType == 1 || a.ToType == 0));
                }
                else
                {
                    linq = linq.Where(a => a.ToType == toType && a.FromUid == id && a.ParentID == ParentID);
                }
            }
            else
            {
                linq = linq.Where(a => a.FromUid == id && a.ParentID == ParentID);
            }
            return linq.OrderByDescending(a => a.CreateDate).ToList(); 
        }
        public List<tMessage> GetPageListWhereFormUidAndloginType(int uid, int loginType, out int count)
        {
            IQueryable<tMessage> linq = dbSet.Where(a => a.FromUid == uid && a.loginType == loginType);

            count = linq.Count();
            return linq.ToList();
        }
        public List<tMessage> GettMessageList()
        { 
            return dbSet.OrderByDescending(a => a.CreateDate).ToList();
        }
        /// <summary>
        /// 留言列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tMessage> GettMessagePageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderBy(a => a.CreateDate)
                .Skip((page - 1) * pagesize).Take(pagesize).ToList(); 
        }
        /// <summary>
        /// 留言列表按id和类型分页
        /// </summary>
        public List<tMessage> GettMessageUidList(int id,int toType, int page, int pagesize, out int count)
        {
            IQueryable<tMessage> ev = dbSet.Where(a => a.Toid == id && a.ToType == toType && a.ParentID == 0).OrderByDescending(a => a.CreateDate);
            count = ev.Count();
            return ev.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<tMessage> GettMessageParentID(int ParentID)
        {
            return dbSet.Where(a => a.ParentID == ParentID).ToList();
        }
        /// <summary>
        /// 根据Uid 获取list列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tMessage> GettMessageUid(int id,int toType)
        {
            return dbSet.Where(a => a.ToUid == id && a.ToType==toType && a.ParentID==0).ToList();
        }
        /// <summary>
        /// 根据Uid 获取Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tMessage GettMessageById(int id)
        {
            return dbSet.Where(a => a.ToUid == id).FirstOrDefault();
        }
        /// <summary>
        /// 查找是否重复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tMessage GettMessageDistinct(int Touid, string strContent, int FromUid)
        {
            return dbSet.Where(a => a.ToUid == Touid && a.sContent == strContent && a.FromUid == FromUid).FirstOrDefault();
        }
        public tMessage GettMessageDistinct(int Touid, string strContent, int FromUid, int ParentID)
        {
            return dbSet.Where(a => a.ToUid == Touid && a.sContent == strContent && a.FromUid == FromUid && a.ParentID==ParentID).FirstOrDefault();
        }
        
        public tMessage GettMessageOnly(int Touid, int FromUid, int ParentID)
        {
            return dbSet.Where(a => a.ToUid == Touid && a.FromUid == FromUid && a.ParentID == ParentID).FirstOrDefault();
        }
        public int updateEntity(tMessage model)
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
