using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;

namespace IYogaKoo.Service.Interfaces
{
    public interface IClassTeacherService
    {
        /// <summary>
        /// 添加课程老师
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add(ViewClassTeacher entity);

        int Delete(int id);
        ViewClassTeacher GetById(int id);
     
        int Delete(string deletelist);

        int AddTeachers(int classId, string teacherIds);
        int EditTeachers(int classId, string teacherIds);

        List<ViewClassTeacher> GetClass_Id(int Class_Id);

        int Edit(ViewClassTeacher entity);

        PageResult<ViewClassTeacher> GetList(string text,int page,int size);
    }
}
