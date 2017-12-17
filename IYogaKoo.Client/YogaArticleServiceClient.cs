using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Client
{

    public class YogaArticleServiceClient : IYogaArticleService, IDisposable
    {
        public IYogaArticleService YogaArticleServiceImpl { get; set; }


        public YogaArticleServiceClient()
        {
            YogaArticleServiceImpl = new YogaArticleServiceImpl(new YogaArticleRepository());
        }
        public List<ViewYogaArticle> GetYogaArticleClassID(int id)
        {
            try
            {
                return YogaArticleServiceImpl.GetYogaArticleClassID(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewYogaArticle> GetYogaArticlePageList(int page, int pagesize, out int count)
        {
            try
            {
                return YogaArticleServiceImpl.GetYogaArticlePageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaArticle> GetYogaArticlePageList(string where, int page, int pagesize, out int count)
        {
            try
            {
                return YogaArticleServiceImpl.GetYogaArticlePageList(where, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewYogaArticle model)
        {
            try
            {
                return YogaArticleServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewYogaArticle GetById(int id)
        {
            try
            {
                return YogaArticleServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewYogaArticle model)
        {
            try
            {
                return YogaArticleServiceImpl.Update(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(string deletelist)
        {
            try
            {
                return YogaArticleServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public List<ViewYogaArticle> GetYogaArticleList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogaArticle> model = new List<ViewYogaArticle>();

                List<ViewYogaArticle> list = GetYogaArticlePageList(page, pagesize, out count);
                 
                //var questionList = from u in list
                //                   where u.ID == null
                //                   select u;

                //var answerList = from u in list
                //                 where u.ID != null
                //                 select u;

                //foreach (var item in questionList)
                //{
                //    tEducationModel temp = new tEducationModel();

                //    //temp.question = item;

                //    //temp.answer = (from u in answerList
                //    //               where u.ID == item.ID
                //    //               select u).FirstOrDefault();

                //    //ViewModelProduct product = new ViewModelProduct();

                //    //using (ProductServiceClient client = new ProductServiceClient())
                //    //{
                //    //    product = client.GetById((int)item.ProductId);
                //    //}
                //    //temp.product = product;
                //    model.Add(temp);
                //}
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<ViewYogaArticle> GetYogaArticleList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogaArticle> model = new List<ViewYogaArticle>();

                List<ViewYogaArticle> list = GetYogaArticlePageList(where,  page, pagesize, out count);
 
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void Dispose()
        {

        }


        public ViewYogaArticle GetYogaArticleById(int id)
        {
            try
            {
                return YogaArticleServiceImpl.GetYogaArticleById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }


        public List<ViewYogaArticle> GetYogaArticlePageList(string strWhere, DateTime? date, int page, int pagesize, out int count)
        {
            try
            {
                return YogaArticleServiceImpl.GetYogaArticlePageList(strWhere, date, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
