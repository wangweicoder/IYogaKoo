using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{
    public interface ItQuestionService
    {
        List<ViewtQuestion> GetList(string whereStr, int page, int pagesize, out int count);

        int Add(ViewtQuestion model);
        //int Update(ViewtQuestion model);
        int Edit(ViewtQuestion model);

        ViewtQuestion GetById(int id);
        int Delete(string deletelist);
    }
}
