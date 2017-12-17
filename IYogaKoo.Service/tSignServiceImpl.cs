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

    public class tSignServiceImpl : ItSignService
    {
        ItSignRepository Repository;
        public tSignServiceImpl(ItSignRepository Repository)
        {
            this.Repository = Repository;
        }
        public ViewtSign GetById(int id)
        {
            return ViewtSign.ToViewModel(Repository.Get(id));
        }
        public int GetCount(string dtTimeNow)
        {
            return  Repository.GetCount(dtTimeNow);
        }
        public bool ExistsSign(int Uid)
        {
            return Repository.ExistsSign(Uid);
        }
        public int RowNums(int Uid)
        {
            return Repository.RowNums(Uid);
        }
        public int Add(ViewtSign model)
        {
            Repository.Add(ViewtSign.ToEntity(model));
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
        public int Update(ViewtSign model)
        {
            Repository.updateEntity(ViewtSign.ToEntity(model));
            return Repository.Save();
        }
        
    }
}
