using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface IYogisModelsService
    {
        List<ViewYogisModels> GetYogisModelsByYogaTypeid(int uid, string YogaTypeid, int count);
         //后台按条件分页查询导师
        List<ViewYogisModels> BackPageList(string RealName, string CenterID, string YogaTypeid, int YogiStatus, int? YogisLevel, int page, int pagesize, out int count);
      
        List<ViewYogisModels> GetYogisModelsList();
        List<ViewYogisModels> GetYogisModelsPageList(int page, int pagesize, out int count);
        List<ViewYogisModels> GetYogisModelsPageListUp(int page, int pagesize, out int count);

        List<ViewYogisModels> GetYogisModelsList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);

        List<ViewYogisModels> GetYogisModelsList(string where, int Gender, int YogisLevel, string YogaTypeid, int Ulevel, int page, int pagesize, out int count);

        List<ViewYogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender,  out int count);
        List<ViewYogisModels> GetYogisModelsList(string strWhere, int DistrictId, int CityId, int PorviceId, int Countryid, int lp, int level, int gender, int page, int pagesize, out int count);

        ViewYogisModels GetYogisModelsById(int id);
        List<ViewYogisModels> GetYogisModelsUid(int id);
        int Add(ViewYogisModels model);

        ViewYogisModels GetById(int id);
        ViewYogisModels GetByRealName(string RealName);
        ViewYogisModels GetWhere(int id, string DisplayImg);
        int Update(ViewYogisModels model);

        int Delete(string deletelist);
        ViewYogisModels Add_Model(ViewYogisModels entity);

        List<ViewYogisModels> GetYogisModelsByCenterId(int centerid, int count);
        /// <summary>
        /// 检查身份证号码
        /// </summary> 
        ViewYogisModels ExistIdCard(string idcardnum);

        DataTable GetSamelevelSupervisor(int YogisLevel);
    }
}
