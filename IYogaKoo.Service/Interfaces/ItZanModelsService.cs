using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface ItZanModelsService
    {
        List<ViewtZanModels> GetByFromUidList(int ToUid, int loginType, out int count);
        List<ViewtZanModels> GetToUidList(int Uid);
        ViewtZanModels GetByiToType(int iToType);
        ViewtZanModels GetExists(int iFromUid, int iToUid, int iType, int iToType);
        int ZanCount(int toUid, int iToType);
        List<ViewtZanModels> GettZanModelsPageListAll();
        List<ViewtZanModels> GettZanModelsPageList(int page, int pagesize, out int count);
        List<ViewtZanModels> GettZanModelsPageList(int ParentID);
        ViewtZanModels GettZanModelsById(int id);
        ViewtZanModels GettZanModelsByClassName(string ClassName);

        List<ViewtZanModels> GettZanModelsUid(int id);
        int Add(ViewtZanModels model);
        List<ViewtZanModels> GettZanUid(int uid);
        ViewtZanModels GetById(int id);
        ViewtZanModels GetByFromToUid(int toid, int fromid, int? iToType);
        int Update(ViewtZanModels model);
        int Count(int toid, int fromid, int? iToType);
        int Delete(string deletelist);
    }
}
