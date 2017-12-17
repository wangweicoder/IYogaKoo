using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;
using IYogaKoo.Service.Interfaces;

namespace IYogaKoo.Service.Interfaces
{
    public interface IInterestedService
    {
        int Count(int classId);
        int Add(ViewInterestedClass interested);

        int Delete(int classId, int userId);
        int Delete(string deletelist);

        PageResult<ViewInterestedClass> Interests(int userId,int page,int size);

        PageResult<ViewInterestedClass> ClassInterests(int classId, int page, int size);

        bool Exists(int classId, int userId);
        ViewInterestedClass GetById(int id);
        List<ViewInterestedClass> GetListClassId(int ClassId);
        List<ViewInterestedClass> GetListClassByUId(int uid);
        List<ViewInterestedClass> GetClassId(int UserId);
        int DeleteNO(int userId, int classId);
    }
}
