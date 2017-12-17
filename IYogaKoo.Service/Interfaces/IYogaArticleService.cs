using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface IYogaArticleService
    {
        List<ViewYogaArticle> GetYogaArticlePageList(int page, int pagesize, out int count);
        List<ViewYogaArticle> GetYogaArticlePageList(string where,  int page, int pagesize, out int count); 
        ViewYogaArticle GetYogaArticleById(int id);
        List<ViewYogaArticle> GetYogaArticleClassID(int id);
        int Add(ViewYogaArticle model);

        ViewYogaArticle GetById(int id);

        int Update(ViewYogaArticle model);

        int Delete(string deletelist);

        List<ViewYogaArticle> GetYogaArticlePageList(string strWhere, DateTime? date, int page, int pagesize, out int count);
    }
}
