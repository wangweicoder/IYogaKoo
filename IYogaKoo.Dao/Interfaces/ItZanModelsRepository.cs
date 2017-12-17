using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface ItZanModelsRepository : IRepository<tZanModels>
    {
        List<tZanModels> GetByFromUidList(int ToUid, int loginType, out int count);
        List<tZanModels> GetToUidList(int Uid);
        tZanModels GetByiToType(int iToType);
        tZanModels GetExists(int iFromUid, int iToUid, int iType, int iToType);
        int ZanCount(int toUid, int iToType);
        List<tZanModels> GettZanModelsPageListAll();
        List<tZanModels> GettZanModelsPageList(int page, int pagesize, out int count);
        List<tZanModels> GettZanModelsPageList(int ParentID);
        List<tZanModels> GettZanModelsUid(int id);
        tZanModels GettZanModelsByClassName(string ClassName);
        List<tZanModels> GettZanUid(int uid);
        tZanModels GettZanModelsById(int id);
        tZanModels GetByFromToUid(int toid, int fromid, int? iToType);
        int Count(int toid, int fromid,int? iToType);
        int updateEntity(tZanModels model);
    }

}
