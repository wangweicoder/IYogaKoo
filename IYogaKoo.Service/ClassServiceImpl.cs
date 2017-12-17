using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IYogaKoo.Service
{
    public class ClassServiceImpl : IClassService
    {
        IClassRepository _repository;
        public ClassServiceImpl(IClassRepository repository)
        {
            _repository = repository;
        }

        public int Add(ViewClass entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.UpdateTime = DateTime.Now;
            Class @class = _repository.Add(ViewClass.ToEntity(entity));
            entity.Id = @class.Id;
            return entity.Id;
        }

        public PageResult<ViewClass> Classes(int page, int size)
        {
            PageResult<ViewClass> pr = new PageResult<ViewClass>(0, "", page, size, 0, null);
            IQueryable<Class> iqueryClass = _repository.Classes.Where(a => a.IsDeleted == false).OrderByDescending(c => c.CreateTime);//.Where(a=>a.IsDeleted==false) qiqi后加的
            pr.RecordCount = iqueryClass.Count();
            IEnumerable<Class> classes = iqueryClass.Skip((page - 1) * size).Take(size).ToList();
            pr.Objects = new List<ViewClass>();

            pr.Objects = (from c in classes select new ViewClass() { Id = c.Id, Name = c.Name, Start = c.Start, Address = c.Address, AreaID = c.AreaID, Price = c.Price, Duration = c.Duration, DurationUnit = c.DurationUnit, Max = c.Max, ClassStatus = c.ClassStatus, CreateTime = c.CreateTime, OrderCount = c.Order.Where(p => p.IsPaid == true || p.Amount == 0).Count(), Creater = new ViewClassTeacher() { UserId = c.UserId, Name = (c.User == null ? "iyogakoo.org" : c.User.NickName) } }).ToList();

            return pr;
        }

        public ViewClass Get(int id)
        {
            return ViewClass.ToViewModel(_repository.Get(id));
        }

        public int AddClassReport(ViewClassReport report)
        {
            throw new NotImplementedException();
        }

        public int AddClassTeacher(ViewClassTeacher entity)
        {
            throw new NotImplementedException();
        }

        public int AddInterestedOfClass(int userId, int classId)
        {
            throw new NotImplementedException();
        }

        public int AddOrder(ViewOrder order)
        {
            throw new NotImplementedException();
        }

        public int AddReportFile(int reportId, ICollection<ViewClassFile> file)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewClass> Classes(Expression<Func<ViewClass, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewClassReport> ClassReports(Expression<Func<ViewClassReport, bool>> predicate)
        {
            throw new NotImplementedException();
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

        public int DeleteClassReport(int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteInterestedOfClass(int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteNoPaidOrder(int userId)
        {
            throw new NotImplementedException();
        }

        public int DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public int Edit(ViewClass entity)
        {
            return _repository.Edit(ViewClass.ToEntity(entity));
        }

        public int EditClassReport(ViewClassReport report)
        {
            throw new NotImplementedException();
        }

        public bool ExistsOrder(int userId, int calssId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewInterestedClass> InterestedClasses(Expression<Func<ViewInterestedClass, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewOrder> Orders(Expression<Func<ViewOrder, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int SetClassStatus(int id, int status, string text)
        {
            Class @class = _repository.Get(id);
            @class.ClassStatus = status;
            @class.MessageDes = text;

            return _repository.Edit(@class); ;
        }
        public int SetClassMany()
        {
            return _repository.SetClassMany();
        }

        public int SetOrderStatus(int orderId, int status)
        {
            throw new NotImplementedException();
        }


        public List<UserListItem> GetAvatars(string uids, int classId, int page, int size)
        {
            return (from y in _repository.GetAvatars(uids, classId, page, size) select new UserListItem() { ID = y.UID, Name = y.RealName, Avatar = y.DisplayImg }).ToList();
        }


        public PageResult<ViewClass> GetClasses(int code, int page, int size, string[] args)
        {
            int count = 0;
            List<Class> list = _repository.GetClasses(code, page, size, out count, args);
            PageResult<ViewClass> pr = new PageResult<ViewClass>(0, "", page, size, count);
            pr.Objects = (from c in list select ViewClass.ToViewModel(c)).ToList();
            return pr;
        }

        


        public List<UserListItem> GetAvatars(string uids)
        {
            return _repository.GetAvatars(uids);
            //return (from y in _repository.GetAvatars(uids) select new UserListItem() { ID = y.UID, Name = y.RealName, Avatar = y.DisplayImg }).ToList();
        }

        public List<DistrictModel> GetDistrictModel(int areaID)
        {
            return _repository.GetDistrictModel(areaID);
        }

        public List<ViewClass> GetClassesByUid(int uid)
        {
            List<Class> list = _repository.GetClassesByUid(uid);
            List<ViewClass> model = new List<ViewClass>();

            foreach (var item in list)
            {
                model.Add(ViewClass.ToViewModel(item));
            }
            return model;
        }

        public List<ViewClass> GetClassesByZhuanYe(int uid, int teacherId, int centerId, int type)
        {
            List<Class> list = _repository.GetClassesByZhuanYe( uid,  teacherId,  centerId,  type);
            List<ViewClass> model = new List<ViewClass>();

            foreach (var item in list)
            {
                model.Add(ViewClass.ToViewModel(item));
            }
            return model;
        }

        public List<ViewClass> GetClassesByUid(string strId, int uid)
        {
            List<Class> list = _repository.GetClassesByUid(strId, uid);
            List<ViewClass> model = new List<ViewClass>();

            foreach (var item in list)
            {
                model.Add(ViewClass.ToViewModel(item));
            }
            return model;
        }

        public List<ViewClassGroup> GetClassHuiGuList(string Orderby, string where, int ClassStatus)
        {
            return _repository.GetClassHuiGuList(Orderby, where, ClassStatus);
        }

        public int GetShoudCloseActivityCount()
        {
            return _repository.GetShoudCloseActivityCount();
        }


        public List<ViewClassGroup> GetClassesHaveReport()
        {
            return _repository.GetClassesHaveReport();
        }
        public List<ViewClass> GetClassesAdvance()
        {
            return _repository.GetClassesAdvance();
        }
    }
}
