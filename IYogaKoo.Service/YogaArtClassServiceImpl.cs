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

    public class YogaArtClassServiceImpl : IYogaArtClassService
    {
        IYogaArtClassRepository Repository;
        public YogaArtClassServiceImpl(IYogaArtClassRepository Repository)
        {
            this.Repository = Repository;
        }
        public List<ViewYogaArtClass> GetYogaArtClassUid(int id)
        {
            List<YogaArtClass> list = Repository.GetYogaArtClassUid(id);

            List<ViewYogaArtClass> model = new List<ViewYogaArtClass>();

            foreach (var item in list)
            {
                model.Add(ViewYogaArtClass.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaArtClass> GetYogaArtClassPageListAll()
        {
            List<YogaArtClass> list = Repository.GetYogaArtClassPageListAll();

            List<ViewYogaArtClass> model = new List<ViewYogaArtClass>();

            foreach (var item in list)
            {
                model.Add(ViewYogaArtClass.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaArtClass> GetYogaArtClassPageList(int page, int pagesize, out int count)
        {
            List<YogaArtClass> list = Repository.GetYogaArtClassPageList(page, pagesize, out count);

            List<ViewYogaArtClass> model = new List<ViewYogaArtClass>();

            foreach (var item in list)
            {
                model.Add(ViewYogaArtClass.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaArtClass> GetYogaArtClassPageList(int ParentID)
        {
            List<YogaArtClass> list = Repository.GetYogaArtClassPageList(ParentID);

            List<ViewYogaArtClass> model = new List<ViewYogaArtClass>();

            foreach (var item in list)
            {
                model.Add(ViewYogaArtClass.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewYogaArtClass model)
        {
            Repository.Add(ViewYogaArtClass.ToEntity(model));
            return Repository.Save();
        }

        public ViewYogaArtClass GetById(int id)
        {
            return ViewYogaArtClass.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewYogaArtClass model)
        {
            Repository.updateEntity(ViewYogaArtClass.ToEntity(model));
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


        public ViewYogaArtClass GetYogaArtClassById(int id)
        {
            return ViewYogaArtClass.ToViewModel(Repository.GetYogaArtClassById(id));
        }

         public ViewYogaArtClass GetYogaArtClassByClassName(string ClassName)
        {
            return ViewYogaArtClass.ToViewModel(Repository.GetYogaArtClassByClassName(ClassName));
        }
    }
}
