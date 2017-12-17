using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao
{
    public class ClassTeacherRepository : Repository<ClassTeacher>,IClassTeacherRepository
    {
        public IQueryable<ClassTeacher> ClassTeachers
        {
            get
            { return dbSet; }
        }

        public List<ClassTeacher> GetClass_Id(int Class_Id)
        {
            return dbSet.Where(a => a.Class_Id == Class_Id).ToList();
        }
    }
}
