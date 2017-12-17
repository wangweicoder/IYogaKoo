using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IYogaKoo.Service
{

    public class YogaDicItemServiceImpl : IYogaDicItemService
    {
        IYogaDicItemRepository Repository;
        public YogaDicItemServiceImpl(IYogaDicItemRepository Repository)
        {
            this.Repository = Repository;
        }
        public List<ViewYogaDicItem> GetDicById(int id)
        {
            List<YogaDicItem> list = Repository.GetDicById(id);

            List<ViewYogaDicItem> model = new List<ViewYogaDicItem>();

            foreach (var item in list)
            {
                model.Add(ViewYogaDicItem.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaDicItem> GetDicId(int id)
        {
            List<YogaDicItem> list = Repository.GetDicId(id);

            List<ViewYogaDicItem> model = new List<ViewYogaDicItem>();

            foreach (var item in list)
            {
                model.Add(ViewYogaDicItem.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaDicItem> GetYogaDicItemList()
        {
            List<YogaDicItem> list = Repository.GetYogaDicItemList();

            List<ViewYogaDicItem> model = new List<ViewYogaDicItem>();

            foreach (var item in list)
            {
                model.Add(ViewYogaDicItem.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaDicItem> GetYogaDicItemPageList(string where,int docid,int page, int pagesize, out int count)
        {
            List<YogaDicItem> list = Repository.GetYogaDicItemPageList(where,docid,page, pagesize, out count);

            List<ViewYogaDicItem> model = new List<ViewYogaDicItem>();

            foreach (var item in list)
            {
                model.Add(ViewYogaDicItem.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaDicItem> GetYogaDicItemPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            List<YogaDicItem> list = Repository.GetYogaDicItemPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

            List<ViewYogaDicItem> model = new List<ViewYogaDicItem>();

            foreach (var item in list)
            {
                model.Add(ViewYogaDicItem.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewYogaDicItem model)
        {
            Repository.Add(ViewYogaDicItem.ToEntity(model));
            return Repository.Save();
        }

        public ViewYogaDicItem GetById(int id)
        {
            return ViewYogaDicItem.ToViewModel(Repository.Get(id));
        }
        public ViewYogaDicItem GetYogaDicItemByItemName(string ItemName)
        {
            return ViewYogaDicItem.ToViewModel(Repository.GetYogaDicItemByItemName(ItemName));
        }
        public int Update(ViewYogaDicItem model)
        {
            Repository.updateEntity(ViewYogaDicItem.ToEntity(model));
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


        public ViewYogaDicItem GetYogaDicItemById(int id)
        {
            return ViewYogaDicItem.ToViewModel(Repository.GetYogaDicItemById(id));
        }

        public List<ViewYogaDicItem> Dics(Expression<Func<ViewYogaDicItem, bool>> predicate)
        {
            var iquery = (from d in Repository.Dics() where d.IsUse==1 && d.IsDelete==0 select new ViewYogaDicItem() { ID=d.ID, ItemName=d.ItemName,DicId=d.DicId });
            return iquery.Where(predicate).ToList();              
        }

        public List<ViewYogaDicItem> GetSelectList(int id, bool? forChild)
        {
            List<YogaDicItem> list = Repository.GetSelectList(id, forChild);
            return (from d in list select new ViewYogaDicItem() { ID = d.ID, DicId = d.DicId, ItemName = d.ItemName }).ToList();
        }
        public List<ViewYogaDicItem> GetSelectList(string ids)
        {
            List<YogaDicItem> list = Repository.GetSelectList(ids);
            return (from d in list select ViewYogaDicItem.ToViewModel(d)).ToList();
        }


        public string GetDicIds(int id)
        {
            return Repository.GetDicIds(id);
        }

        public string GetDicNames(int id)
        {
            return Repository.GetDicNames(id);
        }
    }
}
