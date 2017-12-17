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

    public class tInstationInfoServiceImpl : ItInstationInfoService
    {
        ItInstationInfoRepository Repository;
        public tInstationInfoServiceImpl(ItInstationInfoRepository Repository)
        {
            this.Repository = Repository;
        }

        public List<ViewtInstationInfo> GetPageListWhereUidAndloginType(int uid, int loginType, out int count)
        {
            List<tInstationInfo> list = Repository.GetPageListWhereUidAndloginType(uid, loginType, out count);

            List<ViewtInstationInfo> model = new List<ViewtInstationInfo>();

            foreach (var item in list)
            {
                model.Add(ViewtInstationInfo.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtInstationInfo> GetPageList(int uid, int page, int pagesize, out int count)
        {
            List<tInstationInfo> list = Repository.GetPageList(uid,page, pagesize, out count);

            List<ViewtInstationInfo> model = new List<ViewtInstationInfo>();

            foreach (var item in list)
            {
                model.Add(ViewtInstationInfo.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtInstationInfo> GetPageList(string content, int iType, DateTime? CreateTime, int page, int pagesize, out int count)
        {
            List<tInstationInfo> list = Repository.GetPageList(content, iType, CreateTime, page, pagesize, out count);

            List<ViewtInstationInfo> model = new List<ViewtInstationInfo>();

            foreach (var item in list)
            {
                model.Add(ViewtInstationInfo.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtInstationInfo> GetByContent(string sContent)
        {
            List<tInstationInfo> list = Repository.GetByContent(sContent);

            List<ViewtInstationInfo> model = new List<ViewtInstationInfo>();

            foreach (var item in list)
            {
                model.Add(ViewtInstationInfo.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewtInstationInfo model)
        {
            Repository.Add(ViewtInstationInfo.ToEntity(model));
            return Repository.Save();
        }

        public ViewtInstationInfo GetById(int id)
        {
            return ViewtInstationInfo.ToViewModel(Repository.Get(id));
        }
      
        public ViewtInstationInfo GetByUid(int Uid)
        {
            return ViewtInstationInfo.ToViewModel(Repository.GetByUid(Uid));
        }

        public int Update(ViewtInstationInfo model)
        {
            Repository.updateEntity(ViewtInstationInfo.ToEntity(model));
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
        　
    }
}
