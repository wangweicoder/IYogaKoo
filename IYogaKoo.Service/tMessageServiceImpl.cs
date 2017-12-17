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
    
    public class tMessageServiceImpl : ItMessageService
    {
        ItMessageRepository Repository;
        public tMessageServiceImpl(ItMessageRepository Repository)
        {
            this.Repository = Repository;
        }

        public List<ViewtMessage> GetPageListWhereUidAndloginType(int uid, int loginType, out int count)
        {
            List<tMessage> list = Repository.GetPageListWhereUidAndloginType(uid, loginType,out count);

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtMessage> GetPageListWhereFormUidAndloginType(int uid, int loginType, out int count)
        {
            List<tMessage> list = Repository.GetPageListWhereFormUidAndloginType(uid, loginType,out count); 

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }
        //
        public List<ViewtMessage> GetByMessageFromUid(int toType, int id, int ParentID)
        {
            List<tMessage> list = Repository.GetByMessageFromUid(  toType, id, ParentID);

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtMessage> GetByMessage(int toType, int id, int ParentID)
        {
            List<tMessage> list = Repository.GetByMessage(  toType,  id, ParentID);

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtMessage> GettMessageList()
        {
            List<tMessage> list = Repository.GettMessageList();

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }

        public List<ViewtMessage> GettMessageUid(int id,int toType)
        {
            List<tMessage> list = Repository.GettMessageUid(id,toType);

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtMessage> GettMessageParentID(int ParentID)
        {
            List<tMessage> list = Repository.GettMessageParentID(ParentID);

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }
        public List<ViewtMessage> GettMessagePageList(int page, int pagesize, out int count)
        {
            List<tMessage> list = Repository.GettMessagePageList(page, pagesize, out count);

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }
        /// <summary>
        /// 留言列表按id和类型分页
        /// </summary>
        public List<ViewtMessage> GettMessageUidList(int id,int totype,int page, int pagesize, out int count)
        {
            List<tMessage> list = Repository.GettMessageUidList(id,totype,page, pagesize, out count);

            List<ViewtMessage> model = new List<ViewtMessage>();

            foreach (var item in list)
            {
                model.Add(ViewtMessage.ToViewModel(item));
            }
            return model;
        }

         public int Add(ViewtMessage model)
        {
            Repository.Add(ViewtMessage.ToEntity(model));
            return Repository.Save();
        }

        public ViewtMessage GetById(int id)
        {
            return ViewtMessage.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewtMessage model)
        {
            Repository.updateEntity(ViewtMessage.ToEntity(model));
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


        public ViewtMessage GettMessageById(int id)
        {
            return ViewtMessage.ToViewModel(Repository.GettMessageById(id));
        }

        public ViewtMessage GettMessageDistinct(int Touid, string strContent, int FromUid)
        {
            return ViewtMessage.ToViewModel(Repository.GettMessageDistinct(Touid, strContent, FromUid));
        }
        public ViewtMessage GettMessageDistinct(int Touid, string strContent, int FromUid,int ParentID)
        {
            return ViewtMessage.ToViewModel(Repository.GettMessageDistinct(Touid, strContent, FromUid, ParentID));
        }

         public ViewtMessage GettMessageOnly(int Touid, int FromUid, int ParentID)
        {
            return ViewtMessage.ToViewModel(Repository.GettMessageOnly(Touid, FromUid, ParentID));
        }
    }
}
