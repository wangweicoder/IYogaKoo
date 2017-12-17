using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service
{

    public class tKeyWordServiceImpl : ItKeyWordService
    {
        ItKeyWordRepository Repository;
        public tKeyWordServiceImpl(ItKeyWordRepository Repository)
        {
            this.Repository = Repository;
        }
        public DataTable GetPageListdt(int iType, string where, int page, int pagesize, out int count)
        {
            return Repository.GetPageListdt(iType, where, page, pagesize, out count); 
        }
        public List<ViewtKeyWord> GetPageList(string sWord, int page, int pagesize, out int count)
        {
            List<tKeyWord> list = Repository.GetPageList( sWord,  page,  pagesize, out  count);

            List<ViewtKeyWord> model = new List<ViewtKeyWord>();

            foreach (var item in list)
            {
                model.Add(ViewtKeyWord.ToViewModel(item));
            }
            return model;
        }

        public List<ViewtKeyWord> SearchKeyWordList(string sWord)
        {
            List<tKeyWord> list = Repository.SearchKeyWordList(sWord);

            List<ViewtKeyWord> model = new List<ViewtKeyWord>();

            foreach (var item in list)
            {
                model.Add(ViewtKeyWord.ToViewModel(item));
            }
            return model;
        }

        public int Update(ViewtKeyWord model)
        {
            Repository.updateEntity(ViewtKeyWord.ToEntity(model));
            return Repository.Save();
        }
        public int Add(ViewtKeyWord model)
        {
            Repository.Add(ViewtKeyWord.ToEntity(model));
            return Repository.Save();
        }
        public ViewtKeyWord GetById(int id)
        {
            return ViewtKeyWord.ToViewModel(Repository.Get(id));
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
