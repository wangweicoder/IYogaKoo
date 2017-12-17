using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.Dao.Interfaces;
using IYogaKoo.ViewModel;


namespace IYogaKoo.Service
{
    public class InterestServiceImpl : IInterestedService
    {
         IInterestRepository _repository;
         public InterestServiceImpl(IInterestRepository repository)
        {
            _repository = repository;
        }
         public int Count(int classId)
         {
             return _repository.Count(classId);
         }
         public int Add(ViewInterestedClass interested)
         {
             if (Exists(interested.ClassId, interested.UserId))
                 return Delete(interested.ClassId, interested.UserId);
             else
             {
                 interested.CreateTime = DateTime.Now;
                 InterestedClass ic = _repository.Add(ViewInterestedClass.ToEntity(interested));
                 return ic.Id;
             }
         }

         public int Delete(int classId,int userId)
         {
             return _repository.Delete(classId, userId);
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
         public PageResult<ViewInterestedClass> Interests(int userId,int page, int size)
         {
             var iquery = _repository.Interests().Where(c => c.UserId == userId).OrderBy(c => c.CreateTime);
             PageResult<ViewInterestedClass> pr = new PageResult<ViewInterestedClass>(0, "", page, size, iquery.Count(), new List<ViewInterestedClass>());
             List<InterestedClass> list = iquery.Skip((page - 1) * size).Take(size).ToList();
             foreach (var item in list)
             {
                 item.Class.InterestedClass = null;
             }
             pr.Objects = (from c in list select new ViewInterestedClass() { Id = c.Id, ClassId = c.ClassId, CreateTime = c.CreateTime, Class = new Class() { Banner=c.Class.Banner, Name=c.Class.Name,Start= c.Class.Start,ClassStatus=c.Class.ClassStatus, Summary=c.Class.Summary } }).ToList();
             return pr;
         }

         public PageResult<ViewInterestedClass> ClassInterests(int classId, int page, int size)
         {
             var iquery = _repository.Interests().Where(c => c.ClassId == classId && c.IsDeleted==false).OrderBy(c => c.CreateTime);
             PageResult<ViewInterestedClass> pr = new PageResult<ViewInterestedClass>(0, "", page, size, iquery.Count(), new List<ViewInterestedClass>());
             List<InterestedClass> list = iquery.Skip((page - 1) * size).Take(size).ToList();
             foreach (var item in list)
             {
                 item.Class.InterestedClass = null;
             }
             pr.Objects = (from c in list select new ViewInterestedClass() { Id = c.Id, ClassId = c.ClassId, CreateTime = c.CreateTime, Class = new Class() { Banner = c.Class.Banner, Name = c.Class.Name, Start = c.Class.Start, ClassStatus = c.Class.ClassStatus, Summary = c.Class.Summary } }).ToList();
             return pr;
         }

         public bool Exists(int classId, int userId)
         {
             return _repository.Exists(classId, userId);
         }
         public ViewInterestedClass GetById(int id)
         {
             return ViewInterestedClass.ToViewModel(_repository.Get(id));
         }

         public List<ViewInterestedClass> GetListClassId(int ClassId)
         {
             List<InterestedClass> list = _repository.GetListClassId(ClassId);

             List<ViewInterestedClass> model = new List<ViewInterestedClass>();

             foreach (var item in list)
             {
                 model.Add(ViewInterestedClass.ToViewModel(item));
             }
             return model;
         }


         public List<ViewInterestedClass> GetListClassByUId(int uid)
         {
             List<InterestedClass> list = _repository.GetListClassByUId(uid);

             List<ViewInterestedClass> model = new List<ViewInterestedClass>();

             foreach (var item in list)
             {
                 model.Add(ViewInterestedClass.ToViewModel(item));
             }
             return model;
         }

         public List<ViewInterestedClass> GetClassId(int UserId)
         {
             List<InterestedClass> list = _repository.GetClassId(UserId);

             List<ViewInterestedClass> model = new List<ViewInterestedClass>();

             foreach (var item in list)
             {
                 model.Add(ViewInterestedClass.ToViewModel(item));
             }
             return model;
         }






         public int DeleteNO(int userId, int classId)
         {
             return _repository.DeleteNO(userId, classId);
         }
    }
}
