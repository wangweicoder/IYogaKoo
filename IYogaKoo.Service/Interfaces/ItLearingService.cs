using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface ItLearingService
    {
        List<ViewtLearing> GetExaminePageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count);
         
        List<ViewtLearing> GetPageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count);
        
        List<ViewtLearing> GetPageList(int? iType,int page, int pagesize, out int count);
        List<ViewtLearing> GetPageList_All(int? iType, out int count);
        int Add(ViewtLearing model);
        ViewtLearing ExistsTitle(string Uid, string sTitle);
        ViewtLearing GetById(int id);

        int Update(ViewtLearing model);

        int Delete(string deletelist);
    }
}
