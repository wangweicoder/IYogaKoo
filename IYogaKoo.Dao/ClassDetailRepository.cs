using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{
    public class ClassDetailRepository : Repository<ClassDetail>, IClassDetailRepository   
    {
        public List<ClassDetail> GetClassDetail(string whereStr, int page, int pagesize, out int count)
        {
            whereStr = whereStr.TrimEnd(',');
            string[] whereArray = whereStr.Split(',');
            IQueryable<ClassDetail> linq = dbSet.Where(p=>p.IsDeleted==false).OrderBy(a => a.StartTime);
            count = linq.Count();
            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();

        }

        public int updateEntity(ClassDetail model)
        {

            var entity = dbSet.Find(model.ID);

            if (entity != null)
            {
                Context.Entry(entity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            entity = model;

            Update(entity);

            return Save();


        }
    }
}
