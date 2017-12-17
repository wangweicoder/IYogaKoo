using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service
{

    public class FollowServiceImpl : IFollowService
    {
        IFollowRepository Repository;
        public FollowServiceImpl(IFollowRepository Repository)
        {
            this.Repository = Repository;
        }
        public List<ViewFollow> GetFollowQuiltUidList(int QuiltUid, int loginType, out int count)
        {
            List<Follow> list = Repository.GetFollowQuiltUidList(QuiltUid,loginType,out count);

            List<ViewFollow> model = new List<ViewFollow>();

            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        public List<ViewFollow> GetFollowQuiltUidList(int id)
        {
            List<Follow> list = Repository.GetFollowQuiltUidList(id);

            List<ViewFollow> model = new List<ViewFollow>();

            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        /// <summary>
        /// 瑜伽圈
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>1120</returns>
        public List<ViewFollow> GetFollowUidQuiltList(int uid)
        {
            List<Follow> list = Repository.GetFollowUidQuiltList(uid);
            List<ViewFollow> model = new List<ViewFollow>();
            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        public List<ViewFollow> GetFollowPageList(int page, int pagesize, out int count)
        {
            List<Follow> list = Repository.GetFollowPageList(page, pagesize, out count);

            List<ViewFollow> model = new List<ViewFollow>();

            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        /// <summary>
        /// 粉丝列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<ViewFollow> GetFollowQuiltUidList(int id, int page, int pagesize, out int count)
        {
            List<Follow> list = Repository.GetFollowQuiltUidList(id, page, pagesize, out count);
            List<ViewFollow> model = new List<ViewFollow>();
            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        /// <summary>
        /// 粉丝列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<ViewFollow> GetFollowQuiltUidList(int itype,int id, int page, int pagesize, out int count)
        {
            List<Follow> list = Repository.GetFollowQuiltUidList(itype,id, page, pagesize, out count);
            List<ViewFollow> model = new List<ViewFollow>();
            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        /// <summary>
        /// 关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<ViewFollow> GetFollowUidList(int id, int page, int pagesize, out int count)
        {
            List<Follow> list = Repository.GetFollowUidList(id, page, pagesize, out count);
            List<ViewFollow> model = new List<ViewFollow>();
            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        /// <summary>
        /// 分类的关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        public List<ViewFollow> GetFollowUidList(int itype, int id, int page, int pagesize, out int count)
        {
            List<Follow> list = Repository.GetFollowUidList(itype, id, page, pagesize, out count);
            List<ViewFollow> model = new List<ViewFollow>();
            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        public List<ViewFollow> GetFollowPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            List<Follow> list = Repository.GetFollowPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

            List<ViewFollow> model = new List<ViewFollow>();

            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewFollow model)
        {
            Repository.Add(ViewFollow.ToEntity(model));
            return Repository.Save();
        }

        public ViewFollow GetById(int id)
        {
            return ViewFollow.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewFollow model)
        {
            Repository.updateEntity(ViewFollow.ToEntity(model));
            return Repository.Save();
        }

        public int Delete(string deletelist)
        {
            string[] list = deletelist.TrimEnd(',').Split(',');
            foreach (var item in list)
            {
                Repository.Delete(Repository.Get(int.Parse(item)));
            }
            return Repository.Save();
        }


        public ViewFollow GetFollowById(int id)
        {
            return ViewFollow.ToViewModel(Repository.GetFollowById(id));
        }
        public List<ViewFollow> GetFollowByQuiltUid(int id)
        {
            List<Follow> list = Repository.GetFollowByQuiltUid(id);

            List<ViewFollow> model = new List<ViewFollow>();

            foreach (var item in list)
            {
                model.Add(ViewFollow.ToViewModel(item));
            }
            return model;
        }
        public int GetFollowByCount(int id)
        {
            return Repository.GetFollowByCount(id);
        }
       
         public int GetFollowByUid(int id)
        {
            return Repository.GetFollowByUid(id);
        }

        public ViewFollow GetFollowById(int uid, int QuiltUid)
        {
            return ViewFollow.ToViewModel(Repository.GetFollowById(uid, QuiltUid));
        }         

    }
}
