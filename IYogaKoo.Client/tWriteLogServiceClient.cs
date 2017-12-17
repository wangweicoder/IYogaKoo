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

    public class tWriteLogServiceClient : ItWriteLogService, IDisposable
    {
        public ItWriteLogService tWriteLogServiceImpl { get; set; }
         
        public tWriteLogServiceClient()
        {
            tWriteLogServiceImpl = new tWriteLogServiceImpl(new tWriteLogRepository());
        }

        public List<ViewtWriteLog> BackGetPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count)
        {
            try
            {
                return tWriteLogServiceImpl.BackGetPageList(uid, sTitle, date, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtWriteLog> GettWriteLogQuiltUidList(int id)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogQuiltUidList(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtWriteLog> GettWriteLogImg(int Uid,int ValueType)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogImg(Uid,ValueType);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtWriteLog> GettWriteLogPageList(int page, int pagesize, out int count)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogPageList( page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public List<ViewtWriteLog> GettWriteLogPageList(int uid, int page, int pagesize, out int count)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogPageList(  uid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month, int? day,int page, int pagesize, out int count)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogPageList(uid, Year, Month, day,page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogPageList(uid, Year, Month);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewtWriteLog model)
        {
            try
            {
                return tWriteLogServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtWriteLog GetById(int id)
        {
            try
            {
                return tWriteLogServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewtWriteLog model)
        {
            try
            {
                return tWriteLogServiceImpl.Update(model);
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
                return tWriteLogServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
        public void Dispose()
        {

        }
         
        public ViewtWriteLog GettWriteLogById(int id)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public ViewtWriteLog GettWriteLogById(int uid, int QuiltUid)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogById(uid, QuiltUid);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ViewtWriteLog> GettWriteLogPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogPageList(uid, sTitle, date, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<ViewtWriteLog> GettWriteLogPageList(string urlcontent, DateTime? datetime, int page, int pagesize, out int count)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogPageList(urlcontent,datetime, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month, int page, int pagesize, out int count)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogPageList(uid, Year, Month, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<ViewtWriteLog> GettWriteLogPageListByMessage(int type, int uid, int page, int pagesize, out int count)
        {
            try
            {
                return tWriteLogServiceImpl.GettWriteLogPageListByMessage(type,uid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
