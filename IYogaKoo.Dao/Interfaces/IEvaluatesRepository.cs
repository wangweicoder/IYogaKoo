using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IEvaluatesRepository : IRepository<Evaluates>
    {
        List<Evaluates> GetEvaluatesPageList(int page, int pagesize, out int count);
        List<Evaluates> GetEvaluatesPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);

        Evaluates GetEvaluatesById(int id);
        int updateEntity(Evaluates model);
        Evaluates GetEval(int Touid, string strContent, int FromUid);
        Evaluates GetEval(int Touid, string strContent, int FromUid, int ParentID);
        List<Evaluates> GettEvalUid(int id);
        List<Evaluates> GettEvalUid(int id, int page, int pagesize, out int count);
        void GetRecommendCount(int toid,out int count);
        List<Evaluates> GetEvalParentID(int ParentID);
    }
     
}
