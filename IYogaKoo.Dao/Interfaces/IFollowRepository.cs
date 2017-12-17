using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IFollowRepository : IRepository<Follow>
    {
        List<Follow> GetFollowQuiltUidList(int QuiltUid, int loginType, out int count);
        List<Follow> GetFollowPageList(int page, int pagesize, out int count);
        List<Follow> GetFollowPageList(string strWhere, int Gender, int YogisLevel, string YogaTypeid, int page, int pagesize, out int count);
        /// <summary>
        /// 关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        List<Follow> GetFollowUidList(int id, int page, int pagesize, out int count);
        /// <summary>
        /// 分类的关注列表分页
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        List<Follow> GetFollowUidList(int itype, int id, int page, int pagesize, out int count);
        List<Follow> GetFollowQuiltUidList(int id);
        /// <summary>
        /// 瑜伽圈
        /// </summary>        
        /// <returns>1120</returns>
        List<Follow> GetFollowUidQuiltList(int uid);
        /// <summary>
        /// 关注列表分页
        /// </summary>
        /// <param name="id">quiltuid</param>
        /// <returns></returns>
        List<Follow> GetFollowQuiltUidList(int id, int page, int pagesize, out int count);
        List<Follow> GetFollowQuiltUidList(int itype,int id, int page, int pagesize, out int count);
        Follow GetFollowById(int id);
        Follow GetFollowById(int id, int QuiltUid);
        List<Follow> GetFollowByQuiltUid(int id);
       int GetFollowByCount(int id);   
        int updateEntity(Follow model);
        int GetFollowByUid(int id);
    }
     
}
