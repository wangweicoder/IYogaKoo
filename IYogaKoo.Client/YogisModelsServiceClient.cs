using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Client
{

    public class YogisModelsServiceClient : IYogisModelsService, IDisposable
    {
        public IYogisModelsService YogisModelsServiceImpl { get; set; }
         
        public YogisModelsServiceClient()
        {
            YogisModelsServiceImpl = new YogisModelsServiceImpl(new YogisModelsRepository());
        }

        public List<ViewYogisModels> GetYogisModelsByYogaTypeid(int uid, string YogaTypeid, int count)
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsByYogaTypeid(uid,YogaTypeid,count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //start 后台按条件分页查询导师
        public List<ViewYogisModels> BackPageList(string RealName, string CenterID, string YogaTypeid, int YogiStatus, int? YogisLevel, int page, int pagesize, out int count) 
        {
            try
            {
                return YogisModelsServiceImpl.BackPageList(RealName, CenterID, YogaTypeid, YogiStatus, YogisLevel, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ViewYogisModels GetByRealName(string RealName)
        {
            try
            {
                return YogisModelsServiceImpl.GetByRealName(RealName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //end

        public List<ViewYogisModels> GetYogisModelsList()
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogisModels> GetYogisModelsUid(int id)
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsUid(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogisModels> GetYogisModelsPageList(int page, int pagesize, out int count)
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsPageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogisModels> GetYogisModelsPageListUp(int page, int pagesize, out int count)
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsPageListUp(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewYogisModels model)
        {
            try
            {
                return YogisModelsServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewYogisModels GetById(int id)
        {
            try
            {
                return YogisModelsServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ViewYogisModels GetWhere(int uid, string CoverImg)
        {
            try
            {
                return YogisModelsServiceImpl.GetWhere(uid, CoverImg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(ViewYogisModels model)
        {
            try
            {
                return YogisModelsServiceImpl.Update(model);
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
                return YogisModelsServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
          
        public List<ViewYogisModels> GetYogisModelsList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw;
            }
             
        }
        public List<ViewYogisModels> GetYogisModelsList(string where, int Gender, int YogisLevel, string YogaTypeid, int Ulevel, int page, int pagesize, out int count)
        {
            try
            {

                return YogisModelsServiceImpl.GetYogisModelsList(where, Gender, YogisLevel, YogaTypeid, Ulevel, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void Dispose()
        {

        }


        public ViewYogisModels GetYogisModelsById(int id)
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
       

        public List<ViewYogisModels> GetYogisModelsByCenterId(int centerid, int count)
        {
            try
            {
                List<ViewYogisModels> list = YogisModelsServiceImpl.GetYogisModelsByCenterId(centerid, count);

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        /// <summary>
        /// 检查身份证号码
        /// </summary> 
        public ViewYogisModels ExistIdCard(string idcardnum)
        {
            try
            {
                return YogisModelsServiceImpl.ExistIdCard(idcardnum);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //权重，排序
        public List<ViewYogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender,  out int count)
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, level,  gender,  out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, int page, int pagesize, out int count)
        {
            try
            {
                return YogisModelsServiceImpl.GetYogisModelsList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, level, gender, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ViewYogisModels Add_Model(ViewYogisModels model)
        {
            try
            {
                return YogisModelsServiceImpl.Add_Model(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 随机查询数据
        /// </summary>
        /// <param name="YogisLevel"></param>
        /// <returns></returns>
        public DataTable GetSamelevelSupervisor(int YogisLevel)
        {
            try
            {
                return YogisModelsServiceImpl.GetSamelevelSupervisor(YogisLevel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
