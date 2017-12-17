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

    public class tUserLoginInfoServiceClient : ItUserLoginInfoService, IDisposable
    {
        public ItUserLoginInfoService tUserLoginInfoServiceImpl { get; set; }
         
        public tUserLoginInfoServiceClient()
        {
            tUserLoginInfoServiceImpl = new tUserLoginInfoServiceImpl(new tUserLoginInfoRepository());
        }

        public List<ViewtUserLoginInfo> GetPageList(int page, int pagesize, out int count)
        {
            try
            {
                return tUserLoginInfoServiceImpl.GetPageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       
        public int Add(ViewtUserLoginInfo model)
        {
            try
            {
                return tUserLoginInfoServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewtUserLoginInfo GetById(int id)
        {
            try
            {
                return tUserLoginInfoServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ViewtUserLoginInfo GetByUid(int uid)
        {
            try
            {
                return tUserLoginInfoServiceImpl.GetByUid(uid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(ViewtUserLoginInfo model)
        {
            try
            {
                return tUserLoginInfoServiceImpl.Update(model);
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
                return tUserLoginInfoServiceImpl.Delete(deletelist);
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
