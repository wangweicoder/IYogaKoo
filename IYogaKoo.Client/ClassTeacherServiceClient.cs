using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IYogaKoo.Client
{
    public class ClassTeacherServiceClient : IClassTeacherService, IDisposable
    {
        private IClassTeacherService ClassTeacherServiceImpl { get; set; }

        public ClassTeacherServiceClient()
        {
            ClassTeacherServiceImpl = new ClassTeacherServiceImpl(new ClassTeacherRepository());
        }

        public void Dispose() { }

        public int Add(ViewClassTeacher entity)
        {
            return ClassTeacherServiceImpl.Add(entity);
        }

        public int Delete(int id)
        {
            return ClassTeacherServiceImpl.Delete(id);
        }
        public int Delete(string deletelist)
        {
            try
            {
                return ClassTeacherServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ViewClassTeacher GetById(int id)
        {
            try
            {
                return ClassTeacherServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ViewClassTeacher> GetClass_Id(int Class_Id)
        {
            try
            {
                return ClassTeacherServiceImpl.GetClass_Id(Class_Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int AddTeachers(int classId, string teacherIds)
        {
            return ClassTeacherServiceImpl.AddTeachers(classId, teacherIds);
        }
        public int EditTeachers(int classId, string teacherIds)
        {
            return ClassTeacherServiceImpl.EditTeachers(classId, teacherIds);
        }

        public int Edit(ViewClassTeacher entity)
        {
            return ClassTeacherServiceImpl.Edit(entity);
        }

        public PageResult<ViewClassTeacher> GetList(string text, int page, int size)
        {
            return ClassTeacherServiceImpl.GetList(text, page, size);
        }


    }
}
