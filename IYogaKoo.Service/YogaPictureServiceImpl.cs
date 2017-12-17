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
    public class YogaPictureServiceImpl : IYogaPictureService
    {
         IYogaPictureRepository Repository;
        public YogaPictureServiceImpl(IYogaPictureRepository Repository)
        {
            this.Repository = Repository;
        }
        public ViewYogaPicture ExistsPictureOriginal(int Uid, string PictureOriginal)
        {
            return ViewYogaPicture.ToViewModel(Repository.ExistsPictureOriginal(Uid, PictureOriginal));
        }
        public List<ViewYogaPicture> GetPiclist(int id, string FName)
        {
            List<YogaPicture> list = Repository.GetPiclist(id, FName);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaPicture> GetBackPageList(string Uid, DateTime? createTime, out int count)
        {
            List<YogaPicture> list = Repository.GetBackPageList(Uid, createTime, out   count);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaPicture> GetListWhere(int uid, string fileName)
        {
            List<YogaPicture> list = Repository.GetListWhere(uid, fileName);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaPicture> GetUidList(int id)
        {
            List<YogaPicture> list = Repository.GetUidList(id);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaPicture> GetBackUidList(int id)
        {
            List<YogaPicture> list = Repository.GetBackUidList(id);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaPicture> GetBackUidList(int id,string FName)
        {
            List<YogaPicture> list = Repository.GetBackUidList(id, FName);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogaPicture> GetBackUidList(int id, DateTime create)
        {
            List<YogaPicture> list = Repository.GetBackUidList(id, create);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaPicture> GetBackUidList(int id, string FName, DateTime create)
        {
            List<YogaPicture> list = Repository.GetBackUidList(id,FName, create);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogaPicture> GetYogaPicturePageList(int Nums)
        {
            List<YogaPicture> list = Repository.GetYogaPicturePageList(Nums);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaPicture> GetYogaPictureXiLian(int Nums)
        {
            List<YogaPicture> list = Repository.GetYogaPicturePageList(Nums);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
         
        public List<ViewYogaPicture> GetYogaPicturePageList(int page, int pagesize, out int count)
        {
            List<YogaPicture> list = Repository.GetYogaPicturePageList(page, pagesize, out count);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
         public List<ViewYogaPicture> GetYogaPicturePageList(int uid, int page, int pagesize, out int count)
        {
            List<YogaPicture> list = Repository.GetYogaPicturePageList(uid,page, pagesize, out count);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewYogaPicture model)
        {
            Repository.Add(ViewYogaPicture.ToEntity(model));
            return Repository.Save();
        }

        public ViewYogaPicture GetById(int id)
        {
            return ViewYogaPicture.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewYogaPicture model)
        {
            Repository.updateEntity(ViewYogaPicture.ToEntity(model));
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


        public ViewYogaPicture GetYogaPictureById(int id)
        {
            return ViewYogaPicture.ToViewModel(Repository.GetYogaPictureById(id));
        }

        public ViewYogaPicture GetYogaPictureById(int id,string FName)
        {
            return ViewYogaPicture.ToViewModel(Repository.GetYogaPictureById(id, FName));
        }

        public ViewYogaPicture GetYogaPictureByCreateTime(int id, DateTime Create)
        {
            return ViewYogaPicture.ToViewModel(Repository.GetYogaPictureByCreateTime(id, Create));
        }
        public List<ViewYogaPicture> GetListByType(int uid, int typeid)
        {
            List<YogaPicture> list = Repository.GetListByType(uid,typeid);

            List<ViewYogaPicture> model = new List<ViewYogaPicture>();

            foreach (var item in list)
            {
                model.Add(ViewYogaPicture.ToViewModel(item));
            }
            return model;
        }
         
    }
}
