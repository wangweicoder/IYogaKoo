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

    public class tLearingServiceImpl : ItLearingService
    {
        ItLearingRepository Repository;
        public tLearingServiceImpl(ItLearingRepository Repository)
        {
            this.Repository = Repository;
        }
        public ViewtLearing GetById(int id)
        {
            return ViewtLearing.ToViewModel(Repository.Get(id));
        }
        public List<ViewtLearing> GetExaminePageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count)
        {
            List<tLearing> list = Repository.GetExaminePageList(Uid, sTitle, CreateDate, iType, page, pagesize, out count);

            List<ViewtLearing> model = new List<ViewtLearing>();

            foreach (var item in list)
            {
                model.Add(ViewtLearing.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtLearing> GetPageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count)
        {
            List<tLearing> list = Repository.GetPageList(Uid,sTitle,CreateDate,iType,page, pagesize, out count);

            List<ViewtLearing> model = new List<ViewtLearing>();

            foreach (var item in list)
            {
                model.Add(ViewtLearing.ToViewModel(item));
            }
            return model;
        }

        public List<ViewtLearing> GetPageList(int? iType,int page, int pagesize, out int count)
        {
            List<tLearing> list = Repository.GetPageList(iType,page, pagesize, out count);

            List<ViewtLearing> model = new List<ViewtLearing>();

            foreach (var item in list)
            {
                model.Add(ViewtLearing.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtLearing> GetPageList_All(int? iType, out int count)
        {
            List<tLearing> list = Repository.GetPageList_All(iType, out count);

            List<ViewtLearing> model = new List<ViewtLearing>();

            foreach (var item in list)
            {
                model.Add(ViewtLearing.ToViewModel(item));
            }
            return model;
        }
        public ViewtLearing ExistsTitle(string Uid, string sTitle)
        { 
            return ViewtLearing.ToViewModel(Repository.ExistsTitle(Uid, sTitle));
        }
        public int Add(ViewtLearing model)
        {
            Repository.Add(ViewtLearing.ToEntity(model));
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
        public int Update(ViewtLearing model)
        {
            Repository.updateEntity(ViewtLearing.ToEntity(model));
            return Repository.Save();
        }
       

       

       
         
    }
}
