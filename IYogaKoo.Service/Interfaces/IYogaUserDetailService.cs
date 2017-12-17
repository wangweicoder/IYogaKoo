using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface IYogaUserDetailService
    {
        List<ViewYogaUserDetail> BackGetPageList(string RealName_cn, int? Ulevel, string YogaTypeid,
         int? Nationality, int? CountryID, int? ProvinceID, int? CityID, int? DistrictID,
         int page, int pagesize, out int count);
        List<ViewYogaUserDetail> GetYogaUserDetailPageList(int page, int pagesize, out int count);
        List<ViewYogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, out int count);
        
        List<ViewYogaUserDetail> GetYogaUserList(string where, int Gender, string YogaTypeid, int page, int pagesize, out int count);
        List<ViewYogaUserDetail> GetYogaUserList(string where, int Gender, string YogaTypeid ,int Ulevel,  int page, int pagesize, out int count);
   
        /// <summary>
        /// 瑜伽达人/导师
        /// </summary>
        /// <param name="Nums"></param>
        /// <returns></returns>
        List<ViewYogaUserDetail> GetYogaUserDetailPageList(int Nums);
        List<ViewYogaUserDetail> GetUidList(int id);
        ViewYogaUserDetail GetYogaUserDetailById(int id);
        ViewYogaUserDetail GetByRealName(string RealName);
        int Add(ViewYogaUserDetail model);

        ViewYogaUserDetail GetById(int id);
        ViewYogaUserDetail GetWhere(int id, string DisplayImg);
       
        int Update(ViewYogaUserDetail model);

        int Delete(string deletelist);
         List<ViewYogaUserDetail> GetYogaUserList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp,int level, int page,int gender, int pagesize, out int count);
         DataTable GetSamelevelSupervisor(int Ulevel);
    
    }
}
