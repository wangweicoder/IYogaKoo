using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{

    public interface IFollowService
    {
        List<ViewFollow> GetFollowQuiltUidList(int QuiltUid, int loginType, out int count);
        List<ViewFollow> GetFollowPageList(int page, int pagesize, out int count);
        List<ViewFollow> GetFollowPageList(string where, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);

        int Add(ViewFollow model);

        ViewFollow GetById(int id);

        int Update(ViewFollow model);

        int Delete(string deletelist);
        List<ViewFollow> GetFollowQuiltUidList(int id);
        /// <summary>
        /// 瑜伽圈
        /// </summary>        
        List<ViewFollow> GetFollowUidQuiltList(int uid);
        /// <summary>
        /// 粉丝列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        List<ViewFollow> GetFollowQuiltUidList(int id, int page, int pagesize, out int count);

        List<ViewFollow> GetFollowQuiltUidList(int itype,int id, int page, int pagesize, out int count);
        /// <summary>
        /// 关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        List<ViewFollow> GetFollowUidList(int id,int page, int pagesize, out int count);
        /// <summary>
        /// 分类的关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        List<ViewFollow> GetFollowUidList(int itype, int id, int page, int pagesize, out int count);

        List<ViewFollow> GetFollowByQuiltUid(int id);
       int GetFollowByCount(int id); 
        int GetFollowByUid(int id);

        ViewFollow GetFollowById(int id);
        ViewFollow GetFollowById(int uid, int QuiltUid);
    }
}
