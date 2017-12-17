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

    public class YogaPictureServiceClient : IYogaPictureService, IDisposable
    {
        public IYogaPictureService YogaPictureServiceImpl { get; set; }

        public YogaPictureServiceClient()
        {
            YogaPictureServiceImpl = new YogaPictureServiceImpl(new YogaPictureRepository());
        }
        public ViewYogaPicture ExistsPictureOriginal(int Uid, string PictureOriginal)
        {
            try
            {
                return YogaPictureServiceImpl.ExistsPictureOriginal(Uid, PictureOriginal);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaPicture> GetPiclist(int id, string FName)
        {
            try
            {
                return YogaPictureServiceImpl.GetPiclist(id, FName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         public List<ViewYogaPicture> GetBackPageList(string Uid, DateTime? createTime, out int count)
        {
            try
            {
                return YogaPictureServiceImpl.GetBackPageList(Uid, createTime, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaPicture> GetUidList(int id)
        {
            try
            {
                return YogaPictureServiceImpl.GetUidList(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaPicture> GetBackUidList(int id)
        {
            try
            {
                return YogaPictureServiceImpl.GetBackUidList(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewYogaPicture> GetBackUidList(int id, string FName)
        {
            try
            {
                return YogaPictureServiceImpl.GetBackUidList(id, FName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaPicture> GetBackUidList(int id, DateTime create)
        {
            try
            {
                return YogaPictureServiceImpl.GetBackUidList(id, create);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewYogaPicture> GetBackUidList(int id, string FName, DateTime create)
        {
            try
            {
                return YogaPictureServiceImpl.GetBackUidList(id, FName,create);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaPicture> GetListWhere(int uid, string fileName)
        {
            try
            {
                return YogaPictureServiceImpl.GetListWhere(uid, fileName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaPicture> GetYogaPicturePageList(int Nums)
         {
             try
             {
                 return YogaPictureServiceImpl.GetYogaPicturePageList(Nums);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         
        public List<ViewYogaPicture> GetYogaPicturePageList(int page, int pagesize, out int count)
        {
            try
            {
                return YogaPictureServiceImpl.GetYogaPicturePageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewYogaPicture> GetYogaPicturePageList(int uid, int page, int pagesize, out int count)
        {
            try
            {
                return YogaPictureServiceImpl.GetYogaPicturePageList(uid,page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewYogaPicture model)
        {
            try
            {
                return YogaPictureServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewYogaPicture GetById(int id)
        {
            try
            {
                return YogaPictureServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewYogaPicture model)
        {
            try
            {
                return YogaPictureServiceImpl.Update(model);
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
                return YogaPictureServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewYogaPicture> GetyogaUserGroupList(int Nums)
        {
            try
            {
                List<ViewYogaPicture> model = new List<ViewYogaPicture>();

                List<ViewYogaPicture> list = GetYogaPicturePageList(Nums);
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ViewYogaPicture> GetyogaUserGroupList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogaPicture> model = new List<ViewYogaPicture>();

                List<ViewYogaPicture> list = GetYogaPicturePageList(page, pagesize, out count);
 
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
         
        public ViewYogaPicture GetYogaPictureById(int id)
        {
            try
            {
                return YogaPictureServiceImpl.GetYogaPictureById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public ViewYogaPicture GetYogaPictureById(int id,string FName)
        {
            try
            {
                return YogaPictureServiceImpl.GetYogaPictureById(id, FName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ViewYogaPicture GetYogaPictureByCreateTime(int id, DateTime Create)
        {
            try
            {
                return YogaPictureServiceImpl.GetYogaPictureByCreateTime(id, Create);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ViewYogaPicture> GetListByType(int uid, int typeid)
        {
            try
            {
                return YogaPictureServiceImpl.GetListByType(uid, typeid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
