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

    public class tZanModelsServiceImpl : ItZanModelsService
    {
        ItZanModelsRepository Repository;
        public tZanModelsServiceImpl(ItZanModelsRepository Repository)
        {
            this.Repository = Repository;
        }

        public List<ViewtZanModels> GetByFromUidList(int ToUid, int loginType, out int count)
        {
            List<tZanModels> list = Repository.GetByFromUidList(ToUid, loginType, out count);

            List<ViewtZanModels> model = new List<ViewtZanModels>();

            foreach (var item in list)
            {
                model.Add(ViewtZanModels.ToViewModel(item));
            }
            return model;
        }
        /// <summary>
        /// 是否已经赞过
        /// </summary>
        /// <param name="iFromUid"></param>
        /// <param name="iToUid"></param>
        /// <returns></returns>
        public ViewtZanModels GetExists(int iFromUid, int iToUid, int iType, int iToType)
        {
             return ViewtZanModels.ToViewModel(Repository.GetExists(iFromUid,iToUid,iType,iToType));
        }
        public List<ViewtZanModels> GetToUidList(int Uid)
        {
            List<tZanModels> list = Repository.GetToUidList(Uid);

            List<ViewtZanModels> model = new List<ViewtZanModels>();

            foreach (var item in list)
            {
                model.Add(ViewtZanModels.ToViewModel(item));
            }
            return model;
        }
        public ViewtZanModels GetByiToType(int iToType)
        {
            return ViewtZanModels.ToViewModel(Repository.GetByiToType(iToType));
        }

        public int ZanCount(int toUid, int iToType)
        {
            return Repository.ZanCount(toUid, iToType);
        }
        public List<ViewtZanModels> GettZanModelsUid(int id)
        {
            List<tZanModels> list = Repository.GettZanModelsUid(id);

            List<ViewtZanModels> model = new List<ViewtZanModels>();

            foreach (var item in list)
            {
                model.Add(ViewtZanModels.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtZanModels> GettZanUid(int uid)
        {
            List<tZanModels> list = Repository.GettZanUid(uid);

            List<ViewtZanModels> model = new List<ViewtZanModels>();

            foreach (var item in list)
            {
                model.Add(ViewtZanModels.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtZanModels> GettZanModelsPageListAll()
        {
            List<tZanModels> list = Repository.GettZanModelsPageListAll();

            List<ViewtZanModels> model = new List<ViewtZanModels>();

            foreach (var item in list)
            {
                model.Add(ViewtZanModels.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtZanModels> GettZanModelsPageList(int page, int pagesize, out int count)
        {
            List<tZanModels> list = Repository.GettZanModelsPageList(page, pagesize, out count);

            List<ViewtZanModels> model = new List<ViewtZanModels>();

            foreach (var item in list)
            {
                model.Add(ViewtZanModels.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtZanModels> GettZanModelsPageList(int ParentID)
        {
            List<tZanModels> list = Repository.GettZanModelsPageList(ParentID);

            List<ViewtZanModels> model = new List<ViewtZanModels>();

            foreach (var item in list)
            {
                model.Add(ViewtZanModels.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewtZanModels model)
        {
            Repository.Add(ViewtZanModels.ToEntity(model));
            return Repository.Save();
        }

        public ViewtZanModels GetById(int id)
        {
            return ViewtZanModels.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewtZanModels model)
        {
            Repository.updateEntity(ViewtZanModels.ToEntity(model));
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
        public int Count(int toid, int fromid,int? iToType)
        {
            return Repository.Count(toid, fromid, iToType);
        }

        public ViewtZanModels GettZanModelsById(int id)
        {
            return ViewtZanModels.ToViewModel(Repository.GettZanModelsById(id));
        }

        public ViewtZanModels GettZanModelsByClassName(string ClassName)
        {
            return ViewtZanModels.ToViewModel(Repository.GettZanModelsByClassName(ClassName));
        }
        public ViewtZanModels GetByFromToUid(int toid, int fromid, int? iToType)
        {
            return ViewtZanModels.ToViewModel(Repository.GetByFromToUid(toid, fromid, iToType));
        }
    }
}
