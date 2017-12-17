using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao
{
    public class ClassFileRepository :Repository<ClassFile>,IClassFileRepository
    {
        public bool AddList(List<ClassFile> files)
        {
            Expression<Func<ClassFile,bool>> where =f=>f.ReportId==files[0].ReportId && f.Type==files[0].Type;
            var had = dbSet.Where(where.Compile());
            foreach (var item in had)
            {
                Delete(item);
            }
            foreach (var item in files)
            {
                Add(item);
            }
            int i = Save();
            return i > 0;
        }
        /// <summary>
        /// 根据ReportId 获取list 
        /// </summary>
        /// <param name="ReportId"></param>
        /// <returns></returns>
        public List<ClassFile> GettReportId(int ReportId)
        {
            return dbSet.Where(a => a.ReportId == ReportId).ToList();
        }
    }
}
