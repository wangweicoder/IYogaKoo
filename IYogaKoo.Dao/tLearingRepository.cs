using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class tLearingRepository : Repository<tLearing>, ItLearingRepository
    {

        /// <summary>
        /// 后台列表
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="sTitle"></param>
        /// <param name="CreateDate"></param>
        /// <param name="iType"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tLearing> GetPageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count)
        {
            IQueryable<tLearing> linq =dbSet.OrderByDescending(a => a.CreateDate);
            if (!string.IsNullOrEmpty(Uid))
            {
                linq = linq.Where(x => x.Uid == Uid);
            }
            if (!string.IsNullOrEmpty(sTitle))
            {
                linq = linq.Where(x => x.sTitle == sTitle);
            }
            if (CreateDate != null)
            {
                linq = linq.Where(x => x.CreateDate == CreateDate);
            }
            if (iType != null)
            {
                linq = linq.Where(x => x.iType == iType.Value);
            }
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 审核列表
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="sTitle"></param>
        /// <param name="CreateDate"></param>
        /// <param name="iType"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tLearing> GetExaminePageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count)
        {
            IQueryable<tLearing> linq = dbSet.Where(x=>x.ifexamine==false).OrderByDescending(a => a.CreateDate);
            if (!string.IsNullOrEmpty(Uid))
            {
                linq = linq.Where(x => x.Uid == Uid);
            }
            if (!string.IsNullOrEmpty(sTitle))
            {
                linq = linq.Where(x => x.sTitle == sTitle);
            }
            if (CreateDate != null)
            {
                linq = linq.Where(x => x.CreateDate == CreateDate);
            }
            if (iType != null)
            {
                linq = linq.Where(x => x.iType == iType.Value);
            }
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
       
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="iType">YogaDicItem DicId=2158</param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tLearing> GetPageList(int? iType,int page, int pagesize, out int count)
        {
            IQueryable<tLearing> linq =dbSet.Where(x=>x.ifexamine==true).OrderByDescending(a => a.CreateDate);
            if (iType != 0)
            {
                linq = linq.Where(x => x.iType == iType.Value);
            }
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 查询全部社区
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<tLearing> GetPageList_All(int? iType, out int count)
        {
            IQueryable<tLearing> linq = dbSet.OrderByDescending(a => a.CreateDate);
            if (iType != 0)
            {
                linq = linq.Where(x => x.iType == iType.Value);
            }
            count = linq.Count();

            return linq.ToList();
        }
        /// <summary>
        /// 判断是否存在该文章标题
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="sTitle"></param>
        /// <returns></returns>
        public tLearing ExistsTitle(string Uid,string sTitle)
        {
            IQueryable<tLearing> linq = dbSet.OrderByDescending(a => a.CreateDate);
            if (!string.IsNullOrEmpty(Uid))
            {
                linq = linq.Where(a => a.Uid == Uid);
            }
            return dbSet.Where(a=>a.sTitle==sTitle).FirstOrDefault();
        }

        /// <summary>
        /// 根据主键获取Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tLearing GetById(int id)
        {
            return dbSet.Where(a => a.ID == id).FirstOrDefault();
        }
         
        public int updateEntity(tLearing model)
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
