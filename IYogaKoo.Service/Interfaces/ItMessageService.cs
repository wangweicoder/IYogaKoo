using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface ItMessageService
    {
        List<ViewtMessage> GetPageListWhereUidAndloginType(int uid, int loginType, out int count);
        List<ViewtMessage> GetPageListWhereFormUidAndloginType(int uid, int loginType, out int count);
        List<ViewtMessage> GetByMessageFromUid(int toType, int id, int ParentID);
        List<ViewtMessage> GetByMessage(int toType, int id, int ParentID);
        List<ViewtMessage> GettMessageList();
        List<ViewtMessage> GettMessagePageList(int page, int pagesize, out int count);
        /// <summary>
        /// 留言列表按id和类型分页
        /// </summary>
        List<ViewtMessage> GettMessageUidList(int id, int totype, int page, int pagesize, out int count);
        ViewtMessage GettMessageById(int id);
        List<ViewtMessage> GettMessageUid(int id,int toType);
        List<ViewtMessage> GettMessageParentID(int ParentID);
        ViewtMessage GettMessageDistinct(int Touid, string strContent, int FromUid);
        ViewtMessage GettMessageDistinct(int Touid, string strContent, int FromUid, int ParentID);
       
          ViewtMessage GettMessageOnly(int Touid, int FromUid, int ParentID);
        int Add(ViewtMessage model);

        ViewtMessage GetById(int id);

        int Update(ViewtMessage model);

        int Delete(string deletelist);
    }
}
