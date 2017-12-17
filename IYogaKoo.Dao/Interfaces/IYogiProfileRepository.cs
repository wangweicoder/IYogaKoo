using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IYogiProfileRepository : IRepository<YogiProfile>
    {
        List<YogiProfile> GetYogiProfileList();
        List<YogiProfile> GetYogiProfilePageList(int page, int pagesize, out int count);
        List<YogiProfile> GetYogiProfilePageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);
        List<YogiProfile> GetYogiProfileUid(int id);
        YogiProfile GetYogiProfileById(int id);
        int updateEntity(YogiProfile model);
    }
     
}
