using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class YogaArticleRepository : Repository<YogaArticle>, IYogaArticleRepository
    {
        public List<YogaArticle> GetYogaArticlePageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();
             
            return dbSet.OrderByDescending(a => a.CreateTime).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<YogaArticle> GetYogaArticlePageList(string strWhere,  int page, int pagesize, out int count)
        {
            IQueryable<YogaArticle> linq = dbSet.OrderBy(a => a.CreateTime);

            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.ArticleTitle.Contains(strWhere) || a.Author.Contains(strWhere) || a.ArticleCon.Contains(strWhere) && a.IsDelete!=1);
            }
           
            count = linq.Where(x=>x.ClassID!=7 && x.ClassID!=8 && x.ClassID!=9).Count();// dbSet.Count();

            return linq.Where(x => x.ClassID != 7 && x.ClassID != 8 && x.ClassID != 9).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        /// <summary>
        /// 根据文章表主键ID 获取列表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<YogaArticle> GetYogaArticleClassID(int id)
        {
            return dbSet.Where(a => a.ClassID == id).ToList();
        }
        /// <summary>
        /// 根据主键获取列表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogaArticle GetYogaArticleById(int id)
        {
            return dbSet.Where(a => a.ID == id).FirstOrDefault();
        }


        public int updateEntity(YogaArticle model)
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


        public List<YogaArticle> GetYogaArticlePageList(string strWhere, DateTime? date, int page, int pagesize, out int count)
        {
            IQueryable<YogaArticle> linq = dbSet.OrderByDescending(a => a.CreateTime);
            if (!string.IsNullOrEmpty(strWhere))
            {
                linq = linq.Where(a => a.ArticleTitle.Contains(strWhere));
            }
            if (date != null)
            {
                linq = linq.Where(a => a.CreateTime > date);
            }
            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
    }
}
