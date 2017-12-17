using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{ 
    public interface ItInstationInfoService
    {
        List<ViewtInstationInfo> GetPageListWhereUidAndloginType(int uid, int loginType, out int count);
        List<ViewtInstationInfo> GetPageList(int uid, int page, int pagesize, out int count);
        List<ViewtInstationInfo> GetPageList(string content, int iType, DateTime? CreateTime, int page, int pagesize, out int count);
        List<ViewtInstationInfo> GetByContent(string sContent);
        int Add(ViewtInstationInfo model); 
        ViewtInstationInfo GetById(int id);
        ViewtInstationInfo GetByUid(int Uid);
        int Update(ViewtInstationInfo model);

        int Delete(string deletelist);
    }
}
