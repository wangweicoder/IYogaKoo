using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface ItWriteLogService
    {
        List<ViewtWriteLog> BackGetPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count);
        List<ViewtWriteLog> GettWriteLogPageList(int page, int pagesize, out int count);
        List<ViewtWriteLog> GettWriteLogPageList(int uid, int page, int pagesize, out int count);
        List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month,int? day, int page, int pagesize, out int count);
        List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month);
        List<ViewtWriteLog> GettWriteLogImg(int Uid, int ValueType);
        List<ViewtWriteLog> GettWriteLogPageList(string urlcontent, DateTime? datetime, int page, int pagesize, out int count);
        int Add(ViewtWriteLog model);

        ViewtWriteLog GetById(int id);

        int Update(ViewtWriteLog model);

        int Delete(string deletelist);
        List<ViewtWriteLog> GettWriteLogQuiltUidList(int id);
       
        ViewtWriteLog GettWriteLogById(int id);
        ViewtWriteLog GettWriteLogById(int uid, int QuiltUid);

        List<ViewtWriteLog> GettWriteLogPageList(int uid, string sTitle, DateTime? date, int page, int pagesize, out int count);

        List<ViewtWriteLog> GettWriteLogPageList(int uid, int Year, int Month, int page, int pagesize, out int count);
        List<ViewtWriteLog> GettWriteLogPageListByMessage(int type, int uid, int page, int pagesize, out int count);
    }
}
