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

    public class tBannerServiceClient : ItBannerService, IDisposable
    {
        public ItBannerService tBannerServiceImpl { get; set; }


        public tBannerServiceClient()
        {
            tBannerServiceImpl = new tBannerServiceImpl(new tBannerRepository());
        }
        /// <summary>
        /// 0 首页Banner 1 活动回顾Banner  2 活动预告Banner
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public List<ViewtBanner> GettBannerList(int iType)
        {
            try
            {
                return tBannerServiceImpl.GettBannerList(iType);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtBanner> GettBannerUid(int id)
        {
            try
            {
                return tBannerServiceImpl.GettBannerUid(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtBanner> GettBannerPageList(int page, int pagesize, out int count)
        {
            try
            {
                return tBannerServiceImpl.GettBannerPageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtBanner> GettBannerPageListUp(int page, int pagesize, out int count)
        {
            try
            {
                return tBannerServiceImpl.GettBannerPageListUp(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtBanner> GettBannerPageList(string where,int Gender,int YogisLevel,string YogaTypeid,int page, int pagesize, out int count)
        {
            try
            {
                return tBannerServiceImpl.GettBannerPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewtBanner model)
        {
            try
            {
                return tBannerServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtBanner GetById(int id)
        {
            try
            {
                return tBannerServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewtBanner model)
        {
            try
            {
                return tBannerServiceImpl.Update(model);
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
                return tBannerServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         
        public List<ViewtBanner> GettBannerList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewtBanner> model = new List<ViewtBanner>();

                List<ViewtBanner> list = GettBannerPageList(page, pagesize, out count);
                 
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

        public List<ViewtBanner> GettBannerList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                List<ViewtBanner> model = new List<ViewtBanner>();

                List<ViewtBanner> list = GettBannerPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
 
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


        public ViewtBanner GettBannerById(int id)
        {
            try
            {
                return tBannerServiceImpl.GettBannerById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
