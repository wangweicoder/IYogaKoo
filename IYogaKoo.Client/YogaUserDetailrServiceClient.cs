using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Client
{

    public class YogaUserDetailServiceClient : IYogaUserDetailService, IDisposable
    {
       
        public IYogaUserDetailService YogaUserDetailServiceImpl { get; set; }

        public YogaUserDetailServiceClient()
        {
            YogaUserDetailServiceImpl = new YogaUserDetailServiceImpl(new YogaUserDetailRepository());
        }

        public List<ViewYogaUserDetail> BackGetPageList(string RealName_cn, int? Ulevel, string YogaTypeid,
            int? Nationality, int? CountryID, int? ProvinceID, int? CityID, int? DistrictID,
            int page, int pagesize, out int count)
        {
            try
            {
                return YogaUserDetailServiceImpl.BackGetPageList(RealName_cn, Ulevel,  YogaTypeid,
            Nationality, CountryID, ProvinceID, CityID,DistrictID, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaUserDetail> GetUidList(int id)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetUidList(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaUserDetail> GetYogaUserDetailPageList(int Nums)
         {
             try
             {
                 return YogaUserDetailServiceImpl.GetYogaUserDetailPageList(Nums);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

        public List<ViewYogaUserDetail> GetYogaUserDetailPageList(int page, int pagesize, out int count)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetYogaUserDetailPageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ViewYogaUserDetail GetWhere(int uid, string CoverImg)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetWhere(uid, CoverImg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 权重，因为需要排序，所以取全部数据,根据人气排序
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="DistrictId"></param>
        /// <param name="CityId"></param>
        /// <param name="PorviceId"></param>
        /// <param name="Countryid"></param>
        /// <param name="lp"></param>
        /// <param name="level"></param>
        /// <param name="gender"></param>
        /// <param name="count"></param>
        /// <returns></returns>
       public List<ViewYogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, out int count)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetYogaUserList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, level, gender,  out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaUserDetail> GetYogaUserList(string where, int Gender,   string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetYogaUserList(where, Gender, YogaTypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewYogaUserDetail> GetYogaUserList(string where, int Gender, string YogaTypeid, int Ulevel, int page, int pagesize, out int count)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetYogaUserList(where, Gender, YogaTypeid,Ulevel, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewYogaUserDetail model)
        {
            try
            {
                return YogaUserDetailServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewYogaUserDetail GetById(int id)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ViewYogaUserDetail GetByRealName(string RealName)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetByRealName(RealName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewYogaUserDetail model)
        {
            try
            {
                return YogaUserDetailServiceImpl.Update(model);
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
                return YogaUserDetailServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewYogaUserDetail> GetyogaUserGroupList(int Nums)
        {
            try
            {
                List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

                List<ViewYogaUserDetail> list = GetYogaUserDetailPageList(Nums);
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ViewYogaUserDetail> GetyogaUserGroupList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

                List<ViewYogaUserDetail> list = GetYogaUserDetailPageList(page, pagesize, out count);

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

        public void Dispose()
        {

        }
         
        public ViewYogaUserDetail GetYogaUserDetailById(int id)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetYogaUserDetailById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }  

        public List<ViewYogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp,int level,int gender, int page, int pagesize, out int count)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetYogaUserList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, level,gender, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    
        /// <summary>
        /// 随机查询数据
        /// </summary>
        /// <param name="Ulevel"></param>
        /// <returns></returns>
        public DataTable GetSamelevelSupervisor(int Ulevel)
        {
            try
            {
                return YogaUserDetailServiceImpl.GetSamelevelSupervisor(Ulevel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
