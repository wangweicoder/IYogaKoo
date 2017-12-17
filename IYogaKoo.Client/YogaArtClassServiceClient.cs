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

    public class YogaArtClassServiceClient : IYogaArtClassService, IDisposable
    {
        public IYogaArtClassService YogaArtClassServiceImpl { get; set; }


        public YogaArtClassServiceClient()
        {
            YogaArtClassServiceImpl = new YogaArtClassServiceImpl(new YogaArtClassRepository());
        }
        public List<ViewYogaArtClass> GetYogaArtClassUid(int id)
        {
            try
            {
                return YogaArtClassServiceImpl.GetYogaArtClassUid(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewYogaArtClass> GetYogaArtClassPageListAll()
        {
            try
            {
                return YogaArtClassServiceImpl.GetYogaArtClassPageListAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaArtClass> GetYogaArtClassPageList(int page, int pagesize, out int count)
        {
            try
            {
                return YogaArtClassServiceImpl.GetYogaArtClassPageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaArtClass> GetYogaArtClassPageList(int ParentID)
        {
            try
            {
                return YogaArtClassServiceImpl.GetYogaArtClassPageList(ParentID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewYogaArtClass model)
        {
            try
            {
                return YogaArtClassServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewYogaArtClass GetById(int id)
        {
            try
            {
                return YogaArtClassServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewYogaArtClass model)
        {
            try
            {
                return YogaArtClassServiceImpl.Update(model);
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
                return YogaArtClassServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public List<ViewYogaArtClass> GetYogaArtClassList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogaArtClass> model = new List<ViewYogaArtClass>();

                List<ViewYogaArtClass> list = GetYogaArtClassPageList(page, pagesize, out count);
                 
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

        public List<ViewYogaArtClass> GetYogaArtClassList(int ParentID)
        {
            try
            {
                List<ViewYogaArtClass> model = new List<ViewYogaArtClass>();

                List<ViewYogaArtClass> list = GetYogaArtClassPageList(ParentID);
 
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


        public ViewYogaArtClass GetYogaArtClassById(int id)
        {
            try
            {
                return YogaArtClassServiceImpl.GetYogaArtClassById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public ViewYogaArtClass GetYogaArtClassByClassName(string ClassName)
        {
            try
            {
                return YogaArtClassServiceImpl.GetYogaArtClassByClassName(ClassName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
