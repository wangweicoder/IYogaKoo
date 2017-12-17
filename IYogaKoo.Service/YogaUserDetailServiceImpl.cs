using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service
{ 
    public class YogaUserDetailServiceImpl : IYogaUserDetailService
    { 
        IYogaUserDetailRepository Repository;
        public YogaUserDetailServiceImpl(IYogaUserDetailRepository Repository)
        {
            this.Repository = Repository;
        }

          public List<ViewYogaUserDetail> BackGetPageList(string RealName_cn, int? Ulevel, string YogaTypeid,
            int? Nationality, int? CountryID, int? ProvinceID, int? CityID, int? DistrictID,
            int page, int pagesize, out int count)
        {
            List<YogaUserDetail> list = Repository.BackGetPageList(RealName_cn,  Ulevel,   YogaTypeid,
             Nationality,  CountryID,  ProvinceID,  CityID,  DistrictID, page, pagesize, out count);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;

        }
        public List<ViewYogaUserDetail> GetUidList(int id)
        {
            List<YogaUserDetail> list = Repository.GetUidList(id);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, out int count) 
        {
            List<YogaUserDetail> list = Repository.GetYogaUserList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, level, gender,  out count);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;
             
        }
        public List<ViewYogaUserDetail> GetYogaUserList(string where, int Gender, string YogaTypeid, int page, int pagesize, out int count)
        {
            List<YogaUserDetail> list = Repository.GetYogaUserList(where, Gender,  YogaTypeid, page, pagesize, out count);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;
             
        }

        public ViewYogaUserDetail GetWhere(int id, string DisplayImg)
        {
            return ViewYogaUserDetail.ToViewModel(Repository.GetWhere(id, DisplayImg));
        }

        public List<ViewYogaUserDetail> GetYogaUserList(string where, int Gender, string YogaTypeid, int Ulevel, int page, int pagesize, out int count)
        {
            List<YogaUserDetail> list = Repository.GetYogaUserList(where, Gender, YogaTypeid, Ulevel, page, pagesize, out count);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;
             
        }
        public List<ViewYogaUserDetail> GetYogaUserDetailPageList(int Nums)
        {
            List<YogaUserDetail> list = Repository.GetYogaUserDetailPageList(Nums);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaUserDetail> GetYogaUserDetailXiLian(int Nums)
        {
            List<YogaUserDetail> list = Repository.GetYogaUserDetailPageList(Nums);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogaUserDetail> GetYogaUserDetailPageList(int page, int pagesize, out int count)
        {
            List<YogaUserDetail> list = Repository.GetYogaUserDetailPageList(page, pagesize, out count);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;
        }

        public int Add(ViewYogaUserDetail model)
        {
            Repository.Add(ViewYogaUserDetail.ToEntity(model));
            return Repository.Save();
        }

        public ViewYogaUserDetail GetById(int id)
        {
            return ViewYogaUserDetail.ToViewModel(Repository.Get(id));
        }
        public ViewYogaUserDetail GetByRealName(string RealName)
        {
            return ViewYogaUserDetail.ToViewModel(Repository.GetByRealName(RealName));
        }
        public int Update(ViewYogaUserDetail model)
        {
            Repository.updateEntity(ViewYogaUserDetail.ToEntity(model));
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


        public ViewYogaUserDetail GetYogaUserDetailById(int id)
        {
            return ViewYogaUserDetail.ToViewModel(Repository.GetYogaUserDetailById(id));
        }


        public List<ViewYogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp,int level,int gender, int page, int pagesize, out int count)
        {
            List<YogaUserDetail> list = Repository.GetYogaUserList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp,level,gender, page, pagesize, out count);

            List<ViewYogaUserDetail> model = new List<ViewYogaUserDetail>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUserDetail.ToViewModel(item));
            }
            return model;
        }

        public DataTable GetSamelevelSupervisor(int Ulevel)
        {
            return Repository.GetSamelevelSupervisor(Ulevel); 
        }
  
    }
}
