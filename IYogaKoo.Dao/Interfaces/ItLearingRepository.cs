using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItLearingRepository : IRepository<tLearing>
    {
        List<tLearing> GetExaminePageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count);
        List<tLearing> GetPageList(string Uid, string sTitle, DateTime? CreateDate, int? iType, int page, int pagesize, out int count);
        List<tLearing> GetPageList(int? iType, int page, int pagesize, out int count);
        List<tLearing> GetPageList_All(int? iType, out int count);

        tLearing ExistsTitle(string Uid, string sTitle);
        tLearing GetById(int id);
        int updateEntity(tLearing model);
    }
     
}
