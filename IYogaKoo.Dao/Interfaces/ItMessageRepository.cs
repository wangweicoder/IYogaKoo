using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItMessageRepository : IRepository<tMessage>
    {
        List<tMessage> GetPageListWhereUidAndloginType(int uid, int loginType, out int count);
        List<tMessage> GetPageListWhereFormUidAndloginType(int uid, int loginType, out int count);
        List<tMessage> GetByMessageFromUid(int toType, int id, int ParentID);
        List<tMessage> GetByMessage(int toType, int id, int ParentID);
        List<tMessage> GettMessageList();
        List<tMessage> GettMessagePageList(int page, int pagesize, out int count);
        /// <summary>
        /// 留言列表按id和类型分页
        /// </summary>
        List<tMessage> GettMessageUidList(int id,int totype,int page, int pagesize, out int count);
        List<tMessage> GettMessageUid(int id, int toType);

        List<tMessage> GettMessageParentID(int ParentID);
        tMessage GettMessageOnly(int Touid, int FromUid, int ParentID);
        

        tMessage GettMessageDistinct(int Touid, string strContent, int FromUid);
        tMessage GettMessageDistinct(int Touid, string strContent, int FromUid, int ParentID);
       
        
        tMessage GettMessageById(int id);
        int updateEntity(tMessage model);
    }
     
}
