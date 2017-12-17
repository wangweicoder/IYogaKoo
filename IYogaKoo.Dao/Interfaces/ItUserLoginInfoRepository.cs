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
    public interface ItUserLoginInfoRepository : IRepository<tUserLoginInfo>
    {
        List<tUserLoginInfo> GetPageList(int page, int pagesize, out int count);
        tUserLoginInfo GetByUid(int Uid);
        int updateEntity(tUserLoginInfo model);
    }
}
