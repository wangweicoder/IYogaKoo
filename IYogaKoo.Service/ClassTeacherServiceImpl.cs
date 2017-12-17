using System;
using System.Collections.Generic;
using System.Linq;
using IYogaKoo.Entity;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using IYogaKoo.Dao.Interfaces;

namespace IYogaKoo.Service
{
    public class ClassTeacherServiceImpl : IClassTeacherService
    {
        IClassTeacherRepository _repository;
        public ClassTeacherServiceImpl(IClassTeacherRepository repository)
        {
            _repository = repository;
        }
        public int Add(ViewClassTeacher entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.IsDeleted = false;
            entity.Info = entity.Info ?? "";
            ClassTeacher ct = _repository.Add(ViewClassTeacher.ToEntity(entity));
            entity.Id = ct.Id;
            return entity.Id;
        }

        public int Edit(ViewClassTeacher entity)
        {
            ClassTeacher model = _repository.Get(entity.Id);
            model.Avatar = entity.Avatar;
            _repository.Update(model);
            return _repository.Save();
        }

        public int Delete(int id)
        {
            return 0;
        }

        public ViewClassTeacher GetById(int id)
        {
            return ViewClassTeacher.ToViewModel(_repository.Get(id));
        }
        public List<ViewClassTeacher> GetClass_Id(int Class_Id)
        {
            List<ClassTeacher> list = _repository.GetClass_Id(Class_Id);

            List<ViewClassTeacher> model = new List<ViewClassTeacher>();

            foreach (var item in list)
            {
                model.Add(ViewClassTeacher.ToViewModel(item));
            }
            return model;
        }
        public int Delete(string deletelist)
        {
            string[] list = deletelist.TrimEnd(',').Split(',');
            foreach (var item in list)
            {
                _repository.Delete(_repository.Get(int.Parse(item)));
            }
            return _repository.Save();
        }

        public int AddTeachers(int classId, string teacherIds)
        {
            int[] ids = teacherIds.Split(',').ToIntArray();
            IEnumerable<ClassTeacher> entities = _repository.Get(ct => ct.Class_Id != null && (int)ct.Class_Id == classId);
            for (int i = 0; i < entities.Count(); i++)
            {
                _repository.Delete(entities.ElementAt(i));
            }
            _repository.Save();
            entities = _repository.Get(ct => ids.Contains(ct.Id));
            foreach (var item in entities)
            {
                item.Class_Id = classId;
                _repository.Update(item);
            }
            return _repository.Save();
        }

        public int EditTeachers(int classId, string teacherIds)
        {
            int[] ids = teacherIds.Split(',').ToIntArray();
            IEnumerable<ClassTeacher> entities = _repository.Get(ct => ct.Class_Id != null && (int)ct.Class_Id == classId);
            foreach (var item in entities)
            {
                item.Class_Id = null;
                _repository.Update(item);
            }
            _repository.Save();
            entities = _repository.Get(ct => ids.Contains(ct.Id));
            foreach (var item in entities)
            {
                item.Class_Id = classId;
                _repository.Update(item);
            }
            return _repository.Save();
        }

        public PageResult<ViewClassTeacher> GetList(string text, int page, int size)
        {
            PageResult<ViewClassTeacher> pr = new PageResult<ViewClassTeacher>(0, "", page, size);
            _repository.ClassTeachers.Where(ct => ct.Name.Contains(text) && ct.YogaSystem.Contains(text)).OrderByDescending(ct => ct.CreateTime)
                .OrderBy(ct => ct.Name).Skip((page - 1) * size).Take(size);
            return pr;
        }
    }
}