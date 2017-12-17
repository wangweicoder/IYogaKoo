using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ICentersRepository : IRepository<Centers>
    {
        List<Centers> GetCentersPageList(int page, int pagesize,string centertype, out int count);
        List<Centers> GetCentersPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);
        List<Centers> GetCentersUid(int id);
        List<Centers> GetCentersUid();
        Centers GetCentersByUid(string Uid);
        Centers GetCentersByCenterName(string CenterName);
        Centers GetCentersById(int id);
        int updateEntity(Centers model);
        List<Centers> GetCentersPageList(string strWhere, int DistrictId, int CityId,int PorviceId,int Countryid, int lp, string Centertypeid, int page, int pagesize, out int count);
        List<Centers> GetCentersListByClassCenterID(string classCenterID);
    }
     
}
