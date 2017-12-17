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

    public class YogiProfileServiceClient : IYogiProfileService, IDisposable
    {
        public IYogiProfileService YogiProfileServiceImpl { get; set; }


        public YogiProfileServiceClient()
        {
            YogiProfileServiceImpl = new YogiProfileServiceImpl(new YogiProfileRepository());
        }
        public List<ViewYogiProfile> GetYogiProfileList()
        {
            try
            {
                return YogiProfileServiceImpl.GetYogiProfileList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogiProfile> GetYogiProfileUid(int id)
        {
            try
            {
                return YogiProfileServiceImpl.GetYogiProfileUid(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogiProfile> GetYogiProfilePageList(int page, int pagesize, out int count)
        {
            try
            {
                return YogiProfileServiceImpl.GetYogiProfilePageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogiProfile> GetYogiProfilePageList(string where,int Gender,int YogisLevel,string YogaTypeid,int page, int pagesize, out int count)
        {
            try
            {
                return YogiProfileServiceImpl.GetYogiProfilePageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewYogiProfile model)
        {
            try
            {
                return YogiProfileServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewYogiProfile GetById(int id)
        {
            try
            {
                return YogiProfileServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewYogiProfile model)
        {
            try
            {
                return YogiProfileServiceImpl.Update(model);
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
                return YogiProfileServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public List<ViewYogiProfile> GetYogiProfileList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogiProfile> model = new List<ViewYogiProfile>();

                List<ViewYogiProfile> list = GetYogiProfilePageList(page, pagesize, out count);
                 
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

        public List<ViewYogiProfile> GetYogiProfileList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogiProfile> model = new List<ViewYogiProfile>();

                List<ViewYogiProfile> list = GetYogiProfilePageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
 
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


        public ViewYogiProfile GetYogiProfileById(int id)
        {
            try
            {
                return YogiProfileServiceImpl.GetYogiProfileById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
