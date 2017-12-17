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
    public class InterestServiceClient : IInterestedService,IDisposable
    {
        private IInterestedService Impl { get; set; }

        public InterestServiceClient()
        {
            Impl = new InterestServiceImpl(new InterestRepository());
        }
        public int Count(int classId)
        {
            return Impl.Count(classId);
        }
        public int Add(ViewInterestedClass interested)
        {
            return Impl.Add(interested);
        }

        public int Delete(int classId, int userId)
        {
            return Impl.Delete(classId,userId);
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
        public PageResult<ViewInterestedClass> Interests(int usreId, int page, int size)
        {
            return Impl.Interests(usreId,page, size);
        }
        public List<ViewInterestedClass> GetListClassId(int ClassId)
        {
            try
            {
                return Impl.GetListClassId(ClassId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }   
        public bool Exists(int classId, int userId)
        {
            return Impl.Exists(classId, userId);
        }
        public ViewInterestedClass GetById(int id)
        {
            try
            {
                return Impl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {

        }


        public PageResult<ViewInterestedClass> ClassInterests(int classId, int page, int size)
        {
            return Impl.ClassInterests(classId, page, size);
        }


        public List<ViewInterestedClass> GetListClassByUId(int uid)
        {
            return Impl.GetListClassByUId(uid);
        }

        public List<ViewInterestedClass> GetClassId(int UserId)
        {
            return Impl.GetClassId(UserId);
        }


 

        public int DeleteNO(int userId, int classId)
        {
            try
            {
                return Impl.DeleteNO(userId, classId); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
