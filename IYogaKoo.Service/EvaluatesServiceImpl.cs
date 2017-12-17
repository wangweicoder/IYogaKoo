using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service
{

    public class EvaluatesServiceImpl : IEvaluatesService
    {
        IEvaluatesRepository Repository;
        public EvaluatesServiceImpl(IEvaluatesRepository Repository)
        {
            this.Repository = Repository;
        }


        public List<ViewEvaluates> GetEvaluatesPageList(int page, int pagesize, out int count)
        {
            List<Evaluates> list = Repository.GetEvaluatesPageList(page, pagesize, out count);

            List<ViewEvaluates> model = new List<ViewEvaluates>();

            foreach (var item in list)
            {
                model.Add(ViewEvaluates.ToViewModel(item));
            }
            return model;
        }
        public List<ViewEvaluates> GetEvaluatesPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            List<Evaluates> list = Repository.GetEvaluatesPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);

            List<ViewEvaluates> model = new List<ViewEvaluates>();

            foreach (var item in list)
            {
                model.Add(ViewEvaluates.ToViewModel(item));
            }
            return model;
        }
        public int Add(ViewEvaluates model)
        {
            Repository.Add(ViewEvaluates.ToEntity(model));
            return Repository.Save();
        }

        public ViewEvaluates GetById(int id)
        {
            return ViewEvaluates.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewEvaluates model)
        {
            Repository.updateEntity(ViewEvaluates.ToEntity(model));
            return Repository.Save();
        }

        public int Delete(string deletelist)
        {
            string[] list = deletelist.TrimEnd(',').Split(',');
            foreach (var item in list)
            {
                Repository.Delete(Repository.Get(int.Parse(item)));
            }
            return Repository.Save();
        }


        public ViewEvaluates GetEvaluatesById(int id)
        {
            return ViewEvaluates.ToViewModel(Repository.GetEvaluatesById(id));
        } 

        public ViewEvaluates GettEval(int Eid, string EContent, int FromUid)
        {
            return ViewEvaluates.ToViewModel(Repository.GetEval(Eid, EContent, FromUid)); 
        }
        public ViewEvaluates GettEval(int Eid, string EContent, int FromUid, int ParentID)
        {
            return ViewEvaluates.ToViewModel(Repository.GetEval(Eid, EContent, FromUid, ParentID));
        }

        public List<ViewEvaluates> GettEvalUid(int id)
        {
            List<Evaluates> list = Repository.GettEvalUid(id);

            List<ViewEvaluates> model = new List<ViewEvaluates>();

            foreach (var item in list)
            {
                model.Add(ViewEvaluates.ToViewModel(item));
            }
            return model;
        }
        
        public List<ViewEvaluates> GetEvalParentID(int ParentID)
        {
            List<Evaluates> list = Repository.GetEvalParentID(ParentID);

            List<ViewEvaluates> model = new List<ViewEvaluates>();

            foreach (var item in list)
            {
                model.Add(ViewEvaluates.ToViewModel(item));
            }
            return model;
        } 


        public void GetRecommendCount(int toid, out int count)
        {
           Repository.GetRecommendCount(  toid,out count);
        } 

        public List<ViewEvaluates> GettEvalUid(int id, int page, int pagesize, out int count)
        {
            List<Evaluates> list = Repository.GettEvalUid(id,page, pagesize, out count);

            List<ViewEvaluates> model = new List<ViewEvaluates>();

            foreach (var item in list)
            {
                model.Add(ViewEvaluates.ToViewModel(item));
            }
            return model;
        }
    }
}
