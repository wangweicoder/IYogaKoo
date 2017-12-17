using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface ICentersService
    {
        List<ViewCenters> GetCentersPageList(int page, int pagesize,string centertype, out int count);
        List<ViewCenters> GetCentersPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count); 
        ViewCenters GetCentersById(int id);
        List<ViewCenters> GetCentersUid();
        List<ViewCenters> GetCentersUid(int id);
        int Add(ViewCenters model);
        ViewCenters GetCentersByUid(string Uid);

        ViewCenters GetCentersByCenterName(string CenterName);
        ViewCenters GetById(int id);

        int Update(ViewCenters model);

        int Delete(string deletelist);
        List<ViewCenters> GetCentersPageList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, string Centertypeid, int page, int pagesize, out int count);
        List<ViewCenters> GetCentersListByClassCenterID(string classCenterID);
    }
}
