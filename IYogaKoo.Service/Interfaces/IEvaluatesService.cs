using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface IEvaluatesService
    {
        List<ViewEvaluates> GetEvaluatesPageList(int page, int pagesize, out int count);
        List<ViewEvaluates> GetEvaluatesPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);
        ViewEvaluates GetEvaluatesById(int id);

        int Add(ViewEvaluates model);

        ViewEvaluates GetById(int id);

        int Update(ViewEvaluates model);

        int Delete(string deletelist);

        ViewEvaluates GettEval(int Eid, string EContent, int FromUid);
        ViewEvaluates GettEval(int Eid, string EContent, int FromUid,int ParentID);

        List<ViewEvaluates> GettEvalUid(int id);
        List<ViewEvaluates> GettEvalUid(int id, int page, int pagesize, out int count);

        List<ViewEvaluates> GetEvalParentID(int ParentID);

        void GetRecommendCount(int toid, out int count);

      
    }

}
