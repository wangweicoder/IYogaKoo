using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItWriteLogRepository : IRepository<tWriteLog>
    {
        List<tWriteLog> BackGetPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count);
        List<tWriteLog> GettWriteLogPageList(int page, int pagesize, out int count);
        List<tWriteLog> GettWriteLogPageList(int uid, int page, int pagesize, out int count);
        List<tWriteLog> GettWriteLogPageList(int uid,int Year,int Month, int page, int pagesize, out int count);
        List<tWriteLog> GettWriteLogPageList(int uid, int Year, int Month, int? day, int page, int pagesize, out int count);
        List<tWriteLog> GettWriteLogPageList(int uid, int Year, int Month);
        List<tWriteLog> GettWriteLogImg(int Uid, int ValueType);
        List<tWriteLog> GettWriteLogPageList(string urlcontent,DateTime? datetime, int page, int pagesize, out int count);
        List<tWriteLog> GettWriteLogQuiltUidList(int id);
        tWriteLog GettWriteLogById(int id); 
        tWriteLog GettWriteLogById(int uid, int QuiltUid);
        int updateEntity(tWriteLog model);
        List<tWriteLog> GettWriteLogPageList(int uid, string sTitle,DateTime? date, int page, int pagesize, out int count);
        List<tWriteLog> GettWriteLogPageListByMessage(int type,int uid, int page, int pagesize, out int count);
    }
     
}
