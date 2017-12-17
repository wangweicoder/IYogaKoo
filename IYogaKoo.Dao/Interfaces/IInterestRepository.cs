using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IInterestRepository : IRepository<InterestedClass>
    {
        int Count(int classId);
        int Delete(int classId, int userId);

        IQueryable<InterestedClass> Interests();

        bool Exists(int classId, int userId);
        List<InterestedClass> GetListClassId(int ClassId);
        List<InterestedClass> GetListClassByUId(int uid);
        List<InterestedClass> GetClassId(int UserId);

        int DeleteNO( int userId,int classIds);
    }
}
