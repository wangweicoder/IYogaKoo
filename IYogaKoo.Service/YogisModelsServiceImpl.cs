using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service
{

    public class YogisModelsServiceImpl : IYogisModelsService
    {
        IYogisModelsRepository Repository;
        public YogisModelsServiceImpl(IYogisModelsRepository Repository)
        {
            this.Repository = Repository;
        }

        public List<ViewYogisModels> GetYogisModelsByYogaTypeid(int uid, string YogaTypeid, int count)
        {
            List<YogisModels> list = Repository.GetYogisModelsByYogaTypeid(uid,YogaTypeid, count);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }
        //后台按条件分页查询导师
        public List<ViewYogisModels> BackPageList(string RealName, string CenterID, string YogaTypeid, int YogiStatus, int? YogisLevel, int page, int pagesize, out int count)
        {

            List<YogisModels> list = Repository.BackPageList(RealName, CenterID, YogaTypeid, YogiStatus, YogisLevel, page, pagesize, out count);
            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogisModels> GetYogisModelsList()
        {
            List<YogisModels> list = Repository.GetYogisModelsList();

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogisModels> GetYogisModelsUid(int id)
        {
            List<YogisModels> list = Repository.GetYogisModelsUid(id);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogisModels> GetYogisModelsPageList(int page, int pagesize, out int count)
        {
            List<YogisModels> list = Repository.GetYogisModelsPageList(page, pagesize, out count);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogisModels> GetYogisModelsPageListUp(int page, int pagesize, out int count) 
        {
            List<YogisModels> list = Repository.GetYogisModelsPageListUp(page, pagesize, out count);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogisModels> GetYogisModelsList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            List<YogisModels> list = Repository.GetYogisModelsList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogisModels> GetYogisModelsList(string where, int Gender, int YogisLevel, string YogaTypeid, int Ulevel, int page, int pagesize, out int count)
        {
            List<YogisModels> list = Repository.GetYogisModelsList(where, Gender, YogisLevel, YogaTypeid, Ulevel, page, pagesize, out count);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }
     
        public int Add(ViewYogisModels model)
        {
            Repository.Add(ViewYogisModels.ToEntity(model));
            return Repository.Save();
        }

        public ViewYogisModels Add_Model(ViewYogisModels entity)
        {
            YogisModels user = Repository.Add(ViewYogisModels.ToEntity(entity));
            Repository.Save();
            return ViewYogisModels.ToViewModel(user);
        }

        public ViewYogisModels GetById(int id)
        {
            return ViewYogisModels.ToViewModel(Repository.Get(id));
        }

        public ViewYogisModels GetWhere(int id, string DisplayImg)
        {
            return ViewYogisModels.ToViewModel(Repository.GetWhere(id, DisplayImg));
        }
        public ViewYogisModels GetByRealName(string RealName)
        {
            return ViewYogisModels.ToViewModel(Repository.GetByRealName(RealName));
        }
        public int Update(ViewYogisModels model)
        {
            Repository.updateEntity(ViewYogisModels.ToEntity(model));
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


        public ViewYogisModels GetYogisModelsById(int id)
        {
            return ViewYogisModels.ToViewModel(Repository.GetYogisModelsById(id));
        }


        public List<ViewYogisModels> GetYogisModelsByCenterId(int centerid, int count)
        {
            List<YogisModels> list = Repository.GetYogisModelsByCenterId(centerid,count);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;  
        }
        /// <summary>
        /// 检查身份证号码
        /// </summary> 
        public ViewYogisModels ExistIdCard(string idcardnum)
        { 
          return ViewYogisModels.ToViewModel(Repository.ExistIdCard(idcardnum));      
        }


        public List<ViewYogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, out int count)
        {
            List<YogisModels> list = Repository.GetYogisModelsList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, level, gender,  out count);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, int page, int pagesize, out int count)
        {
            List<YogisModels> list = Repository.GetYogisModelsList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, level, gender, page, pagesize, out count);

            List<ViewYogisModels> model = new List<ViewYogisModels>();

            foreach (var item in list)
            {
                model.Add(ViewYogisModels.ToViewModel(item));
            }
            return model;
        }
        public DataTable GetSamelevelSupervisor(int YogisLevel)
        {
            return Repository.GetSamelevelSupervisor(YogisLevel); 
        }
    }
}
