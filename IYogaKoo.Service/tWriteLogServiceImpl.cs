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

    public class tWriteLogServiceImpl : ItWriteLogService
    {
        ItWriteLogRepository Repository;
        public tWriteLogServiceImpl(ItWriteLogRepository Repository)
        {
            this.Repository = Repository;
        }
          public List<ViewtWriteLog> BackGetPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count)
        {
            List<tWriteLog> list = Repository.BackGetPageList(uid, sTitle,date,page,pagesize,out count);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtWriteLog> GettWriteLogQuiltUidList(int id)
        {
            List<tWriteLog> list = Repository.GettWriteLogQuiltUidList(id);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtWriteLog> GettWriteLogPageList(int page, int pagesize, out int count)
        {
            List<tWriteLog> list = Repository.GettWriteLogPageList(page, pagesize, out count);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtWriteLog> GettWriteLogPageList(int uid, int page, int pagesize, out int count)
        {
            List<tWriteLog> list = Repository.GettWriteLogPageList(uid, page, pagesize, out count);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }

        public List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month,int? day, int page, int pagesize, out int count)
        {
            List<tWriteLog> list = Repository.GettWriteLogPageList(uid,  Year,   Month,day,  page, pagesize, out count);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month)
        {
            List<tWriteLog> list = Repository.GettWriteLogPageList(uid, Year, Month);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtWriteLog> GettWriteLogImg(int Uid, int ValueType)
        {
            List<tWriteLog> list = Repository.GettWriteLogImg(Uid,ValueType);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewtWriteLog model)
        {
            Repository.Add(ViewtWriteLog.ToEntity(model));
            return Repository.Save();
        }

        public ViewtWriteLog GetById(int id)
        {
            return ViewtWriteLog.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewtWriteLog model)
        {
            Repository.updateEntity(ViewtWriteLog.ToEntity(model));
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


        public ViewtWriteLog GettWriteLogById(int id)
        {
            return ViewtWriteLog.ToViewModel(Repository.GettWriteLogById(id));
        }

        public ViewtWriteLog GettWriteLogById(int uid, int QuiltUid)
        {
            return ViewtWriteLog.ToViewModel(Repository.GettWriteLogById(uid, QuiltUid));
        }


        public List<ViewtWriteLog> GettWriteLogPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count)
        {
            List<tWriteLog> list = Repository.GettWriteLogPageList(  uid, sTitle ,date,page,pagesize,out count);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }


        public List<ViewtWriteLog> GettWriteLogPageList(string urlcontent, DateTime? datetime, int page, int pagesize, out int count)
        {
            List<tWriteLog> list = Repository.GettWriteLogPageList(urlcontent,datetime,page, pagesize, out count);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }


        public List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month, int page, int pagesize, out int count)
        {
            List<tWriteLog> list = Repository.GettWriteLogPageList(uid, Year, Month, page, pagesize, out count);

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }

        public List<ViewtWriteLog> GettWriteLogPageListByMessage(int type, int uid, int page, int pagesize, out int count)
        {
            List<tWriteLog> list = Repository.GettWriteLogPageListByMessage(type, uid, page, pagesize, out count);    

            List<ViewtWriteLog> model = new List<ViewtWriteLog>();

            foreach (var item in list)
            {
                model.Add(ViewtWriteLog.ToViewModel(item));
            }
            return model;
        }
    }
}
