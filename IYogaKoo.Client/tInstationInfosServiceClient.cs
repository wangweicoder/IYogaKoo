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

    public class tInstationInfoServiceClient : ItInstationInfoService, IDisposable
    {
        public ItInstationInfoService tInstationInfoServiceImpl { get; set; }
         
        public tInstationInfoServiceClient()
        {
            tInstationInfoServiceImpl = new tInstationInfoServiceImpl(new tInstationInfoRepository());
        }
        public List<ViewtInstationInfo> GetPageListWhereUidAndloginType(int uid, int loginType, out int count)
        {
            try
            {
                return tInstationInfoServiceImpl.GetPageListWhereUidAndloginType(uid, loginType, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //前台调用-desc 
        public List<ViewtInstationInfo> GetPageList(int uid, int page, int pagesize, out int count)
        {
            try
            {
                return tInstationInfoServiceImpl.GetPageList( uid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewtInstationInfo> GetByContent(string sContent)
        {
            try
            {
                return tInstationInfoServiceImpl.GetByContent(sContent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //后台调用
        public List<ViewtInstationInfo> GetPageList(string content, int iType, DateTime? CreateTime, int page, int pagesize, out int count)
        {
            try
            {
                return tInstationInfoServiceImpl.GetPageList(  content ,  iType,   CreateTime,page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       
        public int Add(ViewtInstationInfo model)
        {
            try
            {
                return tInstationInfoServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtInstationInfo GetById(int id)
        {
            try
            {
                return tInstationInfoServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ViewtInstationInfo GetByUid(int uid)
        {
            try
            {
                return tInstationInfoServiceImpl.GetByUid(uid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(ViewtInstationInfo model)
        {
            try
            {
                return tInstationInfoServiceImpl.Update(model);
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
                return tInstationInfoServiceImpl.Delete(deletelist);
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
