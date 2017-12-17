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

    public class tZanModelsServiceClient : ItZanModelsService, IDisposable
    {
        public ItZanModelsService tZanModelsServiceImpl { get; set; }
        
        public tZanModelsServiceClient()
        {
            tZanModelsServiceImpl = new tZanModelsServiceImpl(new tZanModelsRepository());
        }

         public List<ViewtZanModels> GetByFromUidList(int ToUid, int loginType, out int count)
        {
            try
            {
                return tZanModelsServiceImpl.GetByFromUidList(ToUid,loginType,out count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public ViewtZanModels GetByiToType(int iToType)
        {
            try
            {
                return tZanModelsServiceImpl.GetByiToType(iToType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public List<ViewtZanModels> GetToUidList(int Uid)
         {
             try
             {
                 return tZanModelsServiceImpl.GetToUidList(Uid);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public ViewtZanModels GetExists(int iFromUid, int iToUid, int iType, int iToType)
        {
            try
            {
                return tZanModelsServiceImpl.GetExists(iFromUid,iToUid,iType,iToType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public int ZanCount(int toUid, int iToType)
         {
             try
             {
                 return tZanModelsServiceImpl.ZanCount(toUid, iToType);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
        public List<ViewtZanModels> GettZanModelsUid(int id)
        {
            try
            {
                return tZanModelsServiceImpl.GettZanModelsUid(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtZanModels> GettZanUid(int uid)
        {
            try
            {
                return tZanModelsServiceImpl.GettZanUid(uid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtZanModels> GettZanModelsPageListAll()
        {
            try
            {
                return tZanModelsServiceImpl.GettZanModelsPageListAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtZanModels> GettZanModelsPageList(int page, int pagesize, out int count)
        {
            try
            {
                return tZanModelsServiceImpl.GettZanModelsPageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtZanModels> GettZanModelsPageList(int ParentID)
        {
            try
            {
                return tZanModelsServiceImpl.GettZanModelsPageList(ParentID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewtZanModels model)
        {
            try
            {
                return tZanModelsServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtZanModels GetById(int id)
        {
            try
            {
                return tZanModelsServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewtZanModels model)
        {
            try
            {
                return tZanModelsServiceImpl.Update(model);
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
                return tZanModelsServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtZanModels> GettZanModelsList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewtZanModels> model = new List<ViewtZanModels>();

                List<ViewtZanModels> list = GettZanModelsPageList(page, pagesize, out count);

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<ViewtZanModels> GettZanModelsList(int ParentID)
        {
            try
            {
                List<ViewtZanModels> model = new List<ViewtZanModels>();

                List<ViewtZanModels> list = GettZanModelsPageList(ParentID);

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


        public ViewtZanModels GettZanModelsById(int id)
        {
            try
            {
                return tZanModelsServiceImpl.GettZanModelsById(id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ViewtZanModels GettZanModelsByClassName(string ClassName)
        {
            try
            {
                return tZanModelsServiceImpl.GettZanModelsByClassName(ClassName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 根据两个id获取
        /// </summary>       
        public ViewtZanModels GetByFromToUid(int toid, int fromid, int? iToType)
        {
            try
            {
                return tZanModelsServiceImpl.GetByFromToUid(toid, fromid, iToType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 赞Count
        /// </summary>
        /// <param name="toid"></param>
        /// <param name="fromid"></param>
        /// <param name="iToType">被赞类型</param>
        /// <returns></returns>
        public int Count(int toid, int fromid,int? iToType)
        {
            try
            {
                return tZanModelsServiceImpl.Count(toid, fromid,iToType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
