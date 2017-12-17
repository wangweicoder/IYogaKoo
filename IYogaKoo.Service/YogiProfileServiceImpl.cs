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

    public class YogiProfileServiceImpl : IYogiProfileService
    {
        IYogiProfileRepository Repository;
        public YogiProfileServiceImpl(IYogiProfileRepository Repository)
        {
            this.Repository = Repository;
        }

        public List<ViewYogiProfile> GetYogiProfileList()
        {
            List<YogiProfile> list = Repository.GetYogiProfileList();

            List<ViewYogiProfile> model = new List<ViewYogiProfile>();

            foreach (var item in list)
            {
                model.Add(ViewYogiProfile.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogiProfile> GetYogiProfileUid(int id)
        {
            List<YogiProfile> list = Repository.GetYogiProfileUid(id);

            List<ViewYogiProfile> model = new List<ViewYogiProfile>();

            foreach (var item in list)
            {
                model.Add(ViewYogiProfile.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogiProfile> GetYogiProfilePageList(int page, int pagesize, out int count)
        {
            List<YogiProfile> list = Repository.GetYogiProfilePageList(page, pagesize, out count);

            List<ViewYogiProfile> model = new List<ViewYogiProfile>();

            foreach (var item in list)
            {
                model.Add(ViewYogiProfile.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogiProfile> GetYogiProfilePageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            List<YogiProfile> list = Repository.GetYogiProfilePageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

            List<ViewYogiProfile> model = new List<ViewYogiProfile>();

            foreach (var item in list)
            {
                model.Add(ViewYogiProfile.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewYogiProfile model)
        {
            Repository.Add(ViewYogiProfile.ToEntity(model));
            return Repository.Save();
        }

        public ViewYogiProfile GetById(int id)
        {
            return ViewYogiProfile.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewYogiProfile model)
        {
            Repository.updateEntity(ViewYogiProfile.ToEntity(model));
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


        public ViewYogiProfile GetYogiProfileById(int id)
        {
            return ViewYogiProfile.ToViewModel(Repository.GetYogiProfileById(id));
        }
    }
}
