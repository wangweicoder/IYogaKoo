using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItInstationInfoRepository : IRepository<tInstationInfo>
    {
        List<tInstationInfo> GetPageListWhereUidAndloginType(int uid, int loginType, out int count);
        List<tInstationInfo> GetPageList(int uid, int page, int pagesize, out int count);
        List<tInstationInfo> GetPageList(string content, int iType, DateTime? CreateTime, int page, int pagesize, out int count);
        tInstationInfo GetByUid(int Uid);
        List<tInstationInfo> GetByContent(string sContent);
        int updateEntity(tInstationInfo model);
    }
}
