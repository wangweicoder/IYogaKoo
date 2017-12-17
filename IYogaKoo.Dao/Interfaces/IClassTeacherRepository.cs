using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;
using IYogaKoo.Entity;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IClassTeacherRepository :IRepository<ClassTeacher>
    {
        IQueryable<ClassTeacher> ClassTeachers { get; }

        List<ClassTeacher> GetClass_Id(int Class_Id);
    }
}
