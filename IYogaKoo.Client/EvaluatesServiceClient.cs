using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Client
{

    public class EvaluatesServiceClient : IEvaluatesService, IDisposable
    {
        public IEvaluatesService EvaluatesServiceImpl { get; set; }


        public EvaluatesServiceClient()
        {
            EvaluatesServiceImpl = new EvaluatesServiceImpl(new EvaluatesRepository());
        }


        public List<ViewEvaluates> GetEvaluatesPageList(int page, int pagesize, out int count)
        {
            try
            {
                return EvaluatesServiceImpl.GetEvaluatesPageList(page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewEvaluates> GetEvaluatesPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                return EvaluatesServiceImpl.GetEvaluatesPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Add(ViewEvaluates model)
        {
            try
            {
                return EvaluatesServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ViewEvaluates GetById(int id)
        {
            try
            {
                return EvaluatesServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ViewEvaluates model)
        {
            try
            {
                return EvaluatesServiceImpl.Update(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(string deletelist)
        {
            try
            {
                return EvaluatesServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewEvaluates> GetEvaluatesList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewEvaluates> model = new List<ViewEvaluates>();

                List<ViewEvaluates> list = GetEvaluatesPageList(page, pagesize, out count);
                 
                //var questionList = from u in list
                //                   where u.ID == null
                //                   select u;

                //var answerList = from u in list
                //                 where u.ID != null
                //                 select u;

                //foreach (var item in questionList)
                //{
                //    tEducationModel temp = new tEducationModel();

                //    //temp.question = item;

                //    //temp.answer = (from u in answerList
                //    //               where u.ID == item.ID
                //    //               select u).FirstOrDefault();

                //    //ViewModelProduct product = new ViewModelProduct();

                //    //using (ProductServiceClient client = new ProductServiceClient())
                //    //{
                //    //    product = client.GetById((int)item.ProductId);
                //    //}
                //    //temp.product = product;
                //    model.Add(temp);
                //}
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<ViewEvaluates> GetEvaluatesList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            try
            {
                List<ViewEvaluates> model = new List<ViewEvaluates>();

                List<ViewEvaluates> list = GetEvaluatesPageList(where, Gender, YogisLevel, YogaTypeid, page, pagesize, out count);
 
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void Dispose()
        {

        }


        public ViewEvaluates GetEvaluatesById(int id)
        {
            try
            {
                return EvaluatesServiceImpl.GetEvaluatesById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        } 

        public ViewEvaluates GettEval(int Eid, string EContent, int FromUid)
        {
            try
            { 
                return EvaluatesServiceImpl.GettEval(Eid, EContent, FromUid);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ViewEvaluates GettEval(int Eid, string EContent, int FromUid, int ParentID)
        {
            try
            {
                return EvaluatesServiceImpl.GettEval(Eid, EContent, FromUid, ParentID);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ViewEvaluates> GettEvalUid(int id)
        {
            try
            {
                return EvaluatesServiceImpl.GettEvalUid(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 

        public List<ViewEvaluates> GetEvalParentID(int ParentID)
        {
            try
            {
                return EvaluatesServiceImpl.GetEvalParentID(ParentID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 

        public void GetRecommendCount(int toid, out int count)
        {
            EvaluatesServiceImpl.GetRecommendCount(toid, out count);
        }


        public List<ViewEvaluates> GettEvalUid(int id, int page, int pagesize, out int count)
        {
            try
            {
                return EvaluatesServiceImpl.GettEvalUid(id,page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
