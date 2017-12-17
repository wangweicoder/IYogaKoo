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

    public class CentersServiceImpl : ICentersService
    {
        ICentersRepository Repository;
        public CentersServiceImpl(ICentersRepository Repository)
        {
            this.Repository = Repository;
        }

        public List<ViewCenters> GetCentersUid()
        {
            List<Centers> list = Repository.GetCentersUid();

            List<ViewCenters> model = new List<ViewCenters>();

            foreach (var item in list)
            {
                model.Add(ViewCenters.ToViewModel(item));
            }
            return model;
        }

        public List<ViewCenters> GetCentersUid(int id)
        {
            List<Centers> list = Repository.GetCentersUid(id);

            List<ViewCenters> model = new List<ViewCenters>();

            foreach (var item in list)
            {
                model.Add(ViewCenters.ToViewModel(item));
            }
            return model;
        }

        public List<ViewCenters> GetCentersPageList(int page, int pagesize, string centertype, out int count)
        {
            List<Centers> list = Repository.GetCentersPageList(page, pagesize, centertype, out count);

            List<ViewCenters> model = new List<ViewCenters>();

            foreach (var item in list)
            {
                model.Add(ViewCenters.ToViewModel(item));
            }
            return model;
        }
        public List<ViewCenters> GetCentersPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            List<Centers> list = Repository.GetCentersPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

            List<ViewCenters> model = new List<ViewCenters>();

            foreach (var item in list)
            {
                model.Add(ViewCenters.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewCenters model)
        {
            Repository.Add(ViewCenters.ToEntity(model));
            return Repository.Save();
        }

        public ViewCenters GetById(int id)
        {
            return ViewCenters.ToViewModel(Repository.Get(id));
        }
        public ViewCenters GetCentersByUid(string Uid)
        {
            return ViewCenters.ToViewModel(Repository.GetCentersByUid(Uid));
        }

        public ViewCenters GetCentersByCenterName(string CenterName)
        {
            return ViewCenters.ToViewModel(Repository.GetCentersByCenterName(CenterName));
        }
        public int Update(ViewCenters model)
        {
            Repository.updateEntity(ViewCenters.ToEntity(model));
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


        public ViewCenters GetCentersById(int id)
        {
            return ViewCenters.ToViewModel(Repository.GetCentersById(id));
        }


        public List<ViewCenters> GetCentersPageList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, string Centertypeid, int page, int pagesize, out int count)
        {
            List<Centers> list = Repository.GetCentersPageList(strWhere, DistrictId, CityId, PorviceId, Countryid, lp, Centertypeid, page, pagesize, out count);

            List<ViewCenters> model = new List<ViewCenters>();

            foreach (var item in list)
            {
                model.Add(ViewCenters.ToViewModel(item));
            }
            return model;
        }

        public List<ViewCenters> GetCentersListByClassCenterID(string classCenterID)
        {
            List<ViewCenters> model = new List<ViewCenters>();
            var list = Repository.GetCentersListByClassCenterID(classCenterID);
            foreach (var item in list)
            {
                model.Add(ViewCenters.ToViewModel(item));
            }
            return model;
        }
    }
}
