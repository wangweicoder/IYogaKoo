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

    public class tUserLoginInfoServiceImpl : ItUserLoginInfoService
    {
        ItUserLoginInfoRepository Repository;
        public tUserLoginInfoServiceImpl(ItUserLoginInfoRepository Repository)
        {
            this.Repository = Repository;
        }

        
        public List<ViewtUserLoginInfo> GetPageList(int page, int pagesize, out int count)
        {
            List<tUserLoginInfo> list = Repository.GetPageList(page, pagesize, out count);

            List<ViewtUserLoginInfo> model = new List<ViewtUserLoginInfo>();

            foreach (var item in list)
            {
                model.Add(ViewtUserLoginInfo.ToViewModel(item));
            }
            return model;
        }

       　
        public int Add(ViewtUserLoginInfo model)
        {
            Repository.Add(ViewtUserLoginInfo.ToEntity(model));
            return Repository.Save();
        }

        public ViewtUserLoginInfo GetById(int id)
        {
            return ViewtUserLoginInfo.ToViewModel(Repository.Get(id));
        }

        public ViewtUserLoginInfo GetByUid(int Uid)
        {
            return ViewtUserLoginInfo.ToViewModel(Repository.GetByUid(Uid));
        }

        public int Update(ViewtUserLoginInfo model)
        {
            Repository.updateEntity(ViewtUserLoginInfo.ToEntity(model));
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
        　
    }
}
