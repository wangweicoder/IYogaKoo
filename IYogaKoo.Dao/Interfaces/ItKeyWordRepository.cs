using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItKeyWordRepository : IRepository<tKeyWord>
    {
        DataTable GetPageListdt(int iType, string where, int page, int pagesize, out int count);
        List<tKeyWord> GetPageList(string sWord, int page, int pagesize, out int count);
        List<tKeyWord> SearchKeyWordList(string sWord);
        int updateEntity(tKeyWord model);
    }
     
}
