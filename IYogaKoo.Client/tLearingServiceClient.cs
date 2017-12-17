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

    public class tLearingServiceClient : ItLearingService, IDisposable
    {
        public ItLearingService tLearingServiceImpl { get; set; }
         
        public tLearingServiceClient()
        {
            tLearingServiceImpl = new tLearingServiceImpl(new tLearingRepository());
        }

        /// <summary>
        /// 审核列表
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="sTitle"></param>
        /// <param name="CreateDate"></param>
        /// <param name="iType"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ViewtLearing> GetExaminePageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count)
        {
            try
            {
                return tLearingServiceImpl.GetExaminePageList(Uid, sTitle, CreateDate, iType, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtLearing> GetPageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count)
        {
            try
            {
                return tLearingServiceImpl.GetPageList(Uid, sTitle, CreateDate, iType, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtLearing> GetPageList(int? iType,int page, int pagesize, out int count)
        {
            try
            {
                return tLearingServiceImpl.GetPageList(iType,page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewtLearing> GetPageList_All(int? iType, out int count)
        {
            try
            {
                return tLearingServiceImpl.GetPageList_All(iType,out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtLearing ExistsTitle(string Uid, string sTitle)
        {
            try
            {
                return tLearingServiceImpl.ExistsTitle(Uid, sTitle);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewtLearing model)
        {
            try
            {
                return tLearingServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtLearing GetById(int id)
        {
            try
            {
                return tLearingServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewtLearing model)
        {
            try
            {
                return tLearingServiceImpl.Update(model);
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
                return tLearingServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
          
        public void Dispose()
        {

        }
         
    }
}
