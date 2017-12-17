using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IYogaUserDetailRepository : IRepository<YogaUserDetail>
    {
        List<YogaUserDetail> BackGetPageList(string RealName_cn, int? Ulevel, string YogaTypeid,
          int? Nationality, int? CountryID, int? ProvinceID, int? CityID, int? DistrictID,
          int page, int pagesize, out int count);
        List<YogaUserDetail> GetYogaUserDetailPageList(int Nums);
        List<YogaUserDetail> GetUidList(int id);
        List<YogaUserDetail> GetYogaUserDetailPageList(int page, int pagesize, out int count);
        //权重
        List<YogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, out int count);
       
        List<YogaUserDetail> GetYogaUserList(string where, int Gender, string YogaTypeid, int page, int pagesize, out int count);
        List<YogaUserDetail> GetYogaUserList(string where, int Gender, string YogaTypeid, int Ulevel, int page, int pagesize, out int count);
        YogaUserDetail GetWhere(int id, string DisplayImg);
        YogaUserDetail GetByRealName(string RealName);
        YogaUserDetail GetYogaUserDetailById(int id); 
        
        int updateEntity(YogaUserDetail model);

        List<YogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp ,int level,int gender, int page, int pagesize, out int count);

        DataTable GetSamelevelSupervisor(int Ulevel);

    }
     
}
