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
    public class tQuestionServiceImpl : ItQuestionService
    {
        ItQuestionRepository Repository;
        public tQuestionServiceImpl(ItQuestionRepository Repository)
        {
            this.Repository = Repository;
        }
        public List<ViewtQuestion> GetList(string whereStr, int page, int pagesize, out int count)
        {
            List<tQuestion> list = Repository.GetList(whereStr, page, pagesize, out  count);
            List<ViewtQuestion> model = new List<ViewtQuestion>();
            foreach (var item in list)
            {
                model.Add(ViewtQuestion.ToViewModel(item));
            }
            return model;
        }

        public ViewtQuestion GetById(int id)
        {
            return ViewtQuestion.ToViewModel(Repository.Get(id));
        }

        public int Add(ViewtQuestion model)
        {
            Repository.Add(ViewtQuestion.ToEntity(model));
            return Repository.Save();
        }
        public int Edit(ViewtQuestion model)
        {

            Repository.Edit(ViewtQuestion.ToEntity(model));
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
