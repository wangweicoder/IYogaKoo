using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class EvaluatesRepository : Repository<Evaluates>, IEvaluatesRepository
    {
        public List<Evaluates> GetEvaluatesPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();
             
            return dbSet.OrderByDescending(a => a.CreateDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        public List<Evaluates> GetEvaluatesPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count)
        {
            IQueryable<Evaluates> linq = dbSet.OrderBy(a => a.CreateDate);

            //if (!string.IsNullOrEmpty(strWhere))
            //{
            //    linq = linq.Where(a => a.RealName.Contains(strWhere) || a.Street.Contains(strWhere) || a.YogisDepict.Contains(strWhere));
            //}
            //if (Gender != 0 && Gender !=null)
            //{
            //    linq = linq.Where(a => a.Gender == Gender);
            //}
            //if (YogisLevel != 0 && YogisLevel !=null )
            //{
            //    linq=linq.Where(a => a.YogisLevel == YogisLevel);
            //}
            //if (!string.IsNullOrEmpty(YogaTypeid))
            //{
            //   linq=linq.Where(a => a.YogaTypeid.Contains(YogaTypeid));
            //}
            count = linq.Count();// dbSet.Count();
             
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 根据uid 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Evaluates GetEvaluatesById(int id)
        {
            return dbSet.Where(a => a.Evaluateid == id).FirstOrDefault();
        }


        public int updateEntity(Evaluates model)
        {

            var entity = dbSet.Find(model.Evaluateid);

            if (entity != null)
            {
                Context.Entry(entity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            entity = model;

            Update(entity);

            return Save();


        }


        public Evaluates GetEval(int Eid, string EContent, int FromUid)
        {
            return dbSet.Where(a => a.Evaluateid == Eid && a.EContent == EContent && a.FromUid == FromUid).FirstOrDefault();
        }
        public Evaluates GetEval(int Touid, string strContent, int FromUid, int ParentID)
        {
            return dbSet.Where(a => a.Evaluateid == Touid && a.EContent == strContent && a.FromUid == FromUid&&a.ParentID==ParentID).FirstOrDefault();
        }

        public List<Evaluates> GettEvalUid(int id)
        {
            return dbSet.Where(a => a.ToUid == id && a.ParentID == 0).ToList();
        }

        public List<Evaluates> GettEvalUid(int id, int page, int pagesize, out int count)
        {
            IQueryable<Evaluates> ev = dbSet.Where(a => a.ToUid == id && a.ParentID == 0).OrderBy(a=>a.CreateDate);
            count = ev.Count();
            return ev.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        public List<Evaluates> GetEvalParentID(int ParentID)
        {

            return dbSet.Where(a => a.ParentID == ParentID).ToList();
        }


        public void GetRecommendCount(int toid, out int count)
        {
            count = dbSet.Where(a => a.ToUid == toid && a.Recommend == 1).Count();
        }
    }
}
