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

    public class tBannerServiceImpl : ItBannerService
    {
        ItBannerRepository Repository;
        public tBannerServiceImpl(ItBannerRepository Repository)
        {
            this.Repository = Repository;
        }

        public List<ViewtBanner> GettBannerList(int iType)
        {
            List<tBanner> list = Repository.GettBannerList(iType);

            List<ViewtBanner> model = new List<ViewtBanner>();

            foreach (var item in list)
            {
                model.Add(ViewtBanner.ToViewModel(item));
            }
            return model;
        }

        public List<ViewtBanner> GettBannerUid(int id)
        {
            List<tBanner> list = Repository.GettBannerUid(id);

            List<ViewtBanner> model = new List<ViewtBanner>();

            foreach (var item in list)
            {
                model.Add(ViewtBanner.ToViewModel(item));
            }
            return model;
        }

        public List<ViewtBanner> GettBannerPageList(int page, int pagesize, out int count)
        {
            List<tBanner> list = Repository.GettBannerPageList(page, pagesize, out count);

            List<ViewtBanner> model = new List<ViewtBanner>();

            foreach (var item in list)
            {
                model.Add(ViewtBanner.ToViewModel(item));
            }
            return model;
        }

        public List<ViewtBanner> GettBannerPageListUp(int page, int pagesize, out int count) 
        {
            List<tBanner> list = Repository.GettBannerPageListUp(page, pagesize, out count);

            List<ViewtBanner> model = new List<ViewtBanner>();

            foreach (var item in list)
            {
                model.Add(ViewtBanner.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtBanner> GettBannerPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            List<tBanner> list = Repository.GettBannerPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

            List<ViewtBanner> model = new List<ViewtBanner>();

            foreach (var item in list)
            {
                model.Add(ViewtBanner.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewtBanner model)
        {
            Repository.Add(ViewtBanner.ToEntity(model));
            return Repository.Save();
        }

        public ViewtBanner GetById(int id)
        {
            return ViewtBanner.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewtBanner model)
        {
            Repository.updateEntity(ViewtBanner.ToEntity(model));
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


        public ViewtBanner GettBannerById(int id)
        {
            return ViewtBanner.ToViewModel(Repository.GettBannerById(id));
        }
    }
}
