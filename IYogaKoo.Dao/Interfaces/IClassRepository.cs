using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        IQueryable<Class> Classes { get; }

        IQueryable<Class> GetIncludes(params string[] paths);

        Class Get(int id);
        int Count(int id);

        int Edit(Class entity);
        int SetClassMany();

        List<YogisModels> GetAvatars(string uids, int classId, int page, int size);

        List<UserListItem> GetAvatars(string uids);

        List<Class> GetClasses(int code, int page, int size, out int count, params string[] args);
        List<Class> GetClassesByZhuanYe(int uid, int teacherId, int centerId, int type);
        List<DistrictModel> GetDistrictModel(int areaID);

        List<Class> GetClassesByUid(int uid);

        List<Class> GetClassesByUid(string strId, int uid);

        List<ViewClassGroup> GetClassHuiGuList(string Orderby, string where, int ClassStatus);

        int GetShoudCloseActivityCount();

        List<ViewClassGroup> GetClassesHaveReport();

        List<ViewClass> GetClassesAdvance();
    }
}
