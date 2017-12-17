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
    public class ClassServiceClient : IClassService, IDisposable
    {
        private IClassService Impl { get; set; }

        public ClassServiceClient()
        {
            Impl = new ClassServiceImpl(new ClassRepository());
        }

        public void Dispose()
        {

        }

        public ViewClass Get(int id)
        {
            ViewClass vc = Impl.Get(id);
            if (vc.User != null)
            {
                vc.Poster = GetAvatars(vc.User.Uid.ToString())[0];
            }
            return vc;
        }

        public int Add(ViewClass entity)
        {
            return Impl.Add(entity);
        }
        public PageResult<ViewClass> Classes(int page, int size)
        {
            PageResult<ViewClass> pr = Impl.Classes(page, size);
            YogaDicItemServiceClient dicClient = new YogaDicItemServiceClient();
            foreach (var item in pr.Objects)
            {
                item.AreaStr = dicClient.GetDicNames(item.AreaID);
            }
            return pr;
        }
        public int Edit(ViewClass entity)
        {
            return Impl.Edit(entity);
        }

        public int SetClassStatus(int id, int status, string text)
        {
            return Impl.SetClassStatus(id, status,text);
        }
        public int SetClassMany()
        {
            return Impl.SetClassMany();
        }

        public IEnumerable<ViewClass> Classes(Expression<Func<ViewClass, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Delete(string deletelist)
        {
            try
            {
                return Impl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int AddOrder(ViewOrder order)
        {
            throw new NotImplementedException();
        }

        public int DeleteNoPaidOrder(int userId)
        {
            throw new NotImplementedException();
        }

        public bool ExistsOrder(int userId, int calssId)
        {
            throw new NotImplementedException();
        }

        public int DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public int SetOrderStatus(int orderId, int status)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewOrder> Orders(Expression<Func<ViewOrder, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int AddInterestedOfClass(int userId, int classId)
        {
            throw new NotImplementedException();
        }

        public int DeleteInterestedOfClass(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewInterestedClass> InterestedClasses(Expression<Func<ViewInterestedClass, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int AddReportFile(int reportId, ICollection<ViewClassFile> file)
        {
            throw new NotImplementedException();
        }

        public int AddClassReport(ViewClassReport report)
        {
            throw new NotImplementedException();
        }

        public int EditClassReport(ViewClassReport report)
        {
            throw new NotImplementedException();
        }

        public int DeleteClassReport(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewClassReport> ClassReports(Expression<Func<ViewClassReport, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public List<UserListItem> GetAvatars(string uids, int classId, int page, int size)
        {
            return Impl.GetAvatars(uids, classId, page, size);
        }

        public PageResult<ViewClass> GetClasses(int code, int page, int size, string[] args)
        {
            return Impl.GetClasses(code, page, size, args);
        }

        public List<ViewClass> GetClassesByZhuanYe(int uid, int teacherId, int centerId, int type)
        {
            return Impl.GetClassesByZhuanYe(uid, teacherId, centerId, type);
        }
        public Result CanApply(int classID, ViewOrder order)
        {
            return null;
        }


        public List<UserListItem> GetAvatars(string uids)
        {
            return Impl.GetAvatars(uids);
        }

        public List<DistrictModel> GetDistrictModel(int areaID)
        {
            return Impl.GetDistrictModel(areaID);
        }


        public List<ViewClass> GetClassesByUid(int uid)
        {
            try
            {
                return Impl.GetClassesByUid(uid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewClass> GetClassesByUid(string strId, int uid)
        {
            try
            {
                return Impl.GetClassesByUid(strId, uid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ViewClassGroup> GetClassHuiGuList(string Orderby, string where, int ClassStatus)
        {
            try
            {
                return Impl.GetClassHuiGuList(Orderby, where, ClassStatus);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetShoudCloseActivityCount()
        {
            return Impl.GetShoudCloseActivityCount();
        }

        public List<ViewClassGroup> GetClassesHaveReport()
        {
            try
            {
                return Impl.GetClassesHaveReport();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ViewClass> GetClassesAdvance()
        {
            try
            {
                return Impl.GetClassesAdvance();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
