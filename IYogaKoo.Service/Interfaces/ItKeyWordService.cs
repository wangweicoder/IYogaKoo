using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{
    public interface ItKeyWordService
    {
        DataTable GetPageListdt(int iType, string where, int page, int pagesize, out int count);
        List<ViewtKeyWord> GetPageList(string sWord, int page, int pagesize, out int count);
         
        List<ViewtKeyWord> SearchKeyWordList(string sWord);

        int Update(ViewtKeyWord model);

        int Add(ViewtKeyWord model);

        ViewtKeyWord GetById(int id);
         
        int Delete(string deletelist);
    }

}
