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

    public class CentersServiceClient : ICentersService, IDisposable
    {
        public ICentersService CentersServiceImpl { get; set; }


        public CentersServiceClient()
        {
            CentersServiceImpl = new CentersServiceImpl(new CentersRepository());
        }
        public List<ViewCenters> GetCentersUid()
        {
            try
            {
                return CentersServiceImpl.GetCentersUid();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewCenters> GetCentersUid(int id)
        {
            try
            {
                return CentersServiceImpl.GetCentersUid(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewCenters> GetCentersPageList(int page, int pagesize, string centertype, out int count)
        {
            try
            {
                return CentersServiceImpl.GetCentersPageList(page, pagesize, centertype, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewCenters> GetCentersPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                return CentersServiceImpl.GetCentersPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewCenters model)
        {
            try
            {
                return CentersServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewCenters GetById(int id)
        {
            try
            {
                return CentersServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ViewCenters GetCentersByUid(string Uid)
        {
            try
            {
                return CentersServiceImpl.GetCentersByUid(Uid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ViewCenters GetCentersByCenterName(string CenterName)
        {
            try
            {
                return CentersServiceImpl.GetCentersByCenterName(CenterName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(ViewCenters model)
        {
            try
            {
                return CentersServiceImpl.Update(model);
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
                return CentersServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewCenters> GetCentersList(int page, int pagesize, string centertype, out int count)
        {
            try
            {
                List<ViewCenters> model = new List<ViewCenters>();

                List<ViewCenters> list = GetCentersPageList(page, pagesize, centertype, out count);

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

        public List<ViewCenters> GetCentersList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                List<ViewCenters> model = new List<ViewCenters>();

                List<ViewCenters> list = GetCentersPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

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


        public ViewCenters GetCentersById(int id)
        {
            try
            {
                return CentersServiceImpl.GetCentersById(id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ViewCenters> GetCentersPageList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, string Centertypeid, int page, int pagesize, out int count)
        {
            try
            {
                return CentersServiceImpl.GetCentersPageList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, Centertypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ViewCenters> GetCentersListByClassCenterID(string classCenterID)
        {
            try
            {
                return CentersServiceImpl.GetCentersListByClassCenterID(classCenterID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
