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

    public class FollowServiceClient : IFollowService, IDisposable
    {
        public IFollowService FollowServiceImpl { get; set; }
         
        public FollowServiceClient()
        {
            FollowServiceImpl = new FollowServiceImpl(new FollowRepository());
        }
        public List<ViewFollow> GetFollowQuiltUidList(int QuiltUid, int loginType, out int count) 
        {
            try
            {
                return FollowServiceImpl.GetFollowQuiltUidList(QuiltUid,loginType,out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewFollow> GetFollowQuiltUidList(int id)
        {
            try
            {
                return FollowServiceImpl.GetFollowQuiltUidList(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 瑜伽圈
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>1120</returns>
        public List<ViewFollow> GetFollowUidQuiltList(int uid)
        {
            try
            {
                return FollowServiceImpl.GetFollowUidQuiltList(uid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewFollow> GetFollowPageList(int page, int pagesize, out int count)
        {
            try
            {
                return FollowServiceImpl.GetFollowPageList( page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 粉丝列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<ViewFollow> GetFollowQuiltUidList(int id, int page, int pagesize, out int count)
        {
            try
            {
                return FollowServiceImpl.GetFollowQuiltUidList(id, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 粉丝列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<ViewFollow> GetFollowQuiltUidList(int itype,int id, int page, int pagesize, out int count)
        {
            try
            {
                return FollowServiceImpl.GetFollowQuiltUidList(itype,id, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<ViewFollow> GetFollowUidList(int id,int page, int pagesize, out int count)
        {
            try
            {
                return FollowServiceImpl.GetFollowUidList(id,page,pagesize,out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 分类的关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<ViewFollow> GetFollowUidList(int itype, int id, int page, int pagesize, out int count)
        {
            try
            {
                return FollowServiceImpl.GetFollowUidList(itype, id, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewFollow> GetFollowPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                return FollowServiceImpl.GetFollowPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewFollow> GetFollowList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewFollow> model = new List<ViewFollow>();

                List<ViewFollow> list = GetFollowPageList(page, pagesize, out count);

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<ViewFollow> GetFollowList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                List<ViewFollow> model = new List<ViewFollow>();

                List<ViewFollow> list = GetFollowPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public int Add(ViewFollow model)
        {
            try
            {
                return FollowServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewFollow GetById(int id)
        {
            try
            {
                return FollowServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewFollow model)
        {
            try
            {
                return FollowServiceImpl.Update(model);
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
                return FollowServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
        public void Dispose()
        {

        }
         
        public ViewFollow GetFollowById(int id)
        {
            try
            {
                return FollowServiceImpl.GetFollowById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        public int GetFollowByCount(int id)
        {
            try
            {
                return FollowServiceImpl.GetFollowByCount(id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ViewFollow> GetFollowByQuiltUid(int id)
        {
            try
            {
                return FollowServiceImpl.GetFollowByQuiltUid(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        public int GetFollowByUid(int id)
        {
            try
            {
                return FollowServiceImpl.GetFollowByUid(id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ViewFollow GetFollowById(int uid, int QuiltUid)
        {
            try
            {
                return FollowServiceImpl.GetFollowById(uid, QuiltUid);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
