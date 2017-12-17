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
    public interface IYogisModelsRepository : IRepository<YogisModels>
    {
        List<YogisModels> GetYogisModelsByYogaTypeid(int uid, string YogaTypeid, int count);
        //start 后台按条件分页查询导师
        List<YogisModels> BackPageList(string RealName, string CenterID, string YogaTypeid, int YogiStatus, int? YogisLevel, int page, int pagesize, out int count);
        YogisModels GetByRealName(string RealName);
        //end
        List<YogisModels> GetYogisModelsList();
        List<YogisModels> GetYogisModelsPageList(int page, int pagesize, out int count);
        List<YogisModels> GetYogisModelsPageListUp(int page, int pagesize, out int count);
        List<YogisModels> GetYogisModelsList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int Ulevel, int page, int pagesize, out int count);
        List<YogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, int page, int pagesize, out int count);
        List<YogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, out int count);
       
        List<YogisModels> GetYogisModelsList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);
        List<YogisModels> GetYogisModelsUid(int id);
        YogisModels GetYogisModelsById(int id);
        YogisModels GetWhere(int id, string DisplayImg);
        
        int updateEntity(YogisModels model);

        List<YogisModels> GetYogisModelsByCenterId(int centerid, int count);
        /// <summary>
        /// 检查身份证号码
        /// </summary> 
        YogisModels ExistIdCard(string idcardnum);

        DataTable GetSamelevelSupervisor(int YogisLevel);
    }
     
}
