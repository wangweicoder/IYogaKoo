using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service
{

    public class YogaArticleServiceImpl : IYogaArticleService
    {
        IYogaArticleRepository Repository;
        public YogaArticleServiceImpl(IYogaArticleRepository Repository)
        {
            this.Repository = Repository;
        }
        public List<ViewYogaArticle> GetYogaArticleClassID(int id)
        {
            List<YogaArticle> list = Repository.GetYogaArticleClassID(id);

            List<ViewYogaArticle> model = new List<ViewYogaArticle>();

            foreach (var item in list)
            {
                model.Add(ViewYogaArticle.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogaArticle> GetYogaArticlePageList(int page, int pagesize, out int count)
        {
            List<YogaArticle> list = Repository.GetYogaArticlePageList(page, pagesize, out count);

            List<ViewYogaArticle> model = new List<ViewYogaArticle>();

            foreach (var item in list)
            {
                model.Add(ViewYogaArticle.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaArticle> GetYogaArticlePageList(string where,   int page, int pagesize, out int count)
        {
            List<YogaArticle> list = Repository.GetYogaArticlePageList(where,   page, pagesize, out count);

            List<ViewYogaArticle> model = new List<ViewYogaArticle>();

            foreach (var item in list)
            {
                model.Add(ViewYogaArticle.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewYogaArticle model)
        {
            Repository.Add(ViewYogaArticle.ToEntity(model));
            return Repository.Save();
        }

        public ViewYogaArticle GetById(int id)
        {
            return ViewYogaArticle.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewYogaArticle model)
        {
            Repository.updateEntity(ViewYogaArticle.ToEntity(model));
            return Repository.Save();
        }

        public int Delete(string deletelist)
        {
            string[] list = deletelist.TrimEnd(',').Split(',');
            foreach (var item in list)
            {
                Repository.Delete(Repository.Get(int.Parse(item)));
            }
            return Repository.Save();
        }


        public ViewYogaArticle GetYogaArticleById(int id)
        {
            return ViewYogaArticle.ToViewModel(Repository.GetYogaArticleById(id));
        }


        public List<ViewYogaArticle> GetYogaArticlePageList(string strWhere, DateTime? date, int page, int pagesize, out int count)
        {
            List<YogaArticle> list = Repository.GetYogaArticlePageList(strWhere, date, page, pagesize, out count);

            List<ViewYogaArticle> model = new List<ViewYogaArticle>();

            foreach (var item in list)
            {
                model.Add(ViewYogaArticle.ToViewModel(item));
            }
            return model;
        }
    }
}
