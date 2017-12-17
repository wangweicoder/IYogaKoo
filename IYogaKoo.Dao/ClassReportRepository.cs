using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao
{
    public class ClassReportRepository : Repository<ClassReport>, IClassReportRepository
    {
        public IQueryable<ClassReport> Reports
        {
            get { return dbSet.Include("ClassFile"); }
        }

        public List<ClassReport> GetClassId(int ClassId)
        {
            return dbSet.Where(a => a.ClassId == ClassId).ToList();
        }
    }
}
