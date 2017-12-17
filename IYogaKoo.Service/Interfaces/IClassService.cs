using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IYogaKoo.ViewModel;
using System.Linq.Expressions;


namespace IYogaKoo.Service.Interfaces
{
    public interface IClassService
    {
        /// <summary>
        /// 课程契约
        /// 课程、订单、活动报道
        /// </summary>
        #region 课程
        /// <summary>
        /// 添加课程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add(ViewClass entity);

        int Edit(ViewClass entity);

        ViewClass Get(int id);

        /// <summary>
        /// 设置课程状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int SetClassStatus(int id, int status,string text);
        int SetClassMany();

        IEnumerable<ViewClass> Classes(Expression<Func<ViewClass, bool>> predicate);

        PageResult<ViewClass> Classes(int page, int size);

        PageResult<ViewClass> GetClasses(int code, int page, int size, string[] args);
        List<ViewClass> GetClassesByZhuanYe(int uid, int teacherId, int centerId, int type);
        List<UserListItem> GetAvatars(string uids, int classId, int page, int size);

        List<UserListItem> GetAvatars(string uids);
        #endregion

        #region 订单

        int AddOrder(ViewOrder order);

        /// <summary>
        /// 删除超时未支付订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int DeleteNoPaidOrder(int userId);

        /// <summary>
        /// 是否存在已支付订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="calssId"></param>
        /// <returns></returns>
        bool ExistsOrder(int userId, int calssId);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int DeleteOrder(int orderId);

        /// <summary>
        /// 设置订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int SetOrderStatus(int orderId, int status);

        IEnumerable<ViewOrder> Orders(Expression<Func<ViewOrder, bool>> predicate);
        #endregion

        #region 感兴趣
        /// <summary>
        /// 添加感兴趣的课程
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="classId">课程编号</param>
        /// <returns></returns>
        int AddInterestedOfClass(int userId, int classId);

        /// <summary>
        /// 删除感兴趣的课程
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        int DeleteInterestedOfClass(int id);

        IEnumerable<ViewInterestedClass> InterestedClasses(Expression<Func<ViewInterestedClass, bool>> predicate);
        #endregion

        #region 课程文件

        int AddReportFile(int reportId, ICollection<ViewClassFile> file);
        #endregion

        #region 活动报道

        int AddClassReport(ViewClassReport report);

        int EditClassReport(ViewClassReport report);

        int DeleteClassReport(int id);

        IEnumerable<ViewClassReport> ClassReports(Expression<Func<ViewClassReport, bool>> predicate);
        #endregion

        int Delete(string deletelist);

        List<DistrictModel> GetDistrictModel(int areaID);


        List<ViewClass> GetClassesByUid(int uid);

        List<ViewClass> GetClassesByUid(string strId, int uid);

        List<ViewClassGroup> GetClassHuiGuList(string Orderby, string where, int ClassStatus);

        int GetShoudCloseActivityCount();

        List<ViewClassGroup> GetClassesHaveReport();
        List<ViewClass> GetClassesAdvance();
    }
}
