using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IYogaArticleRepository : IRepository<YogaArticle>
    {
        List<YogaArticle> GetYogaArticlePageList(int page, int pagesize, out int count);
        List<YogaArticle> GetYogaArticlePageList(string strWhere,  int page, int pagesize, out int count);
        List<YogaArticle> GetYogaArticleClassID(int id);
        YogaArticle GetYogaArticleById(int id);
        int updateEntity(YogaArticle model);
        List<YogaArticle> GetYogaArticlePageList(string strWhere, DateTime? date, int page, int pagesize, out int count);
    }
     
}
