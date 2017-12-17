using IYogaKoo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao.Interfaces
{
    public interface IYogaUserRepository : IRepository<YogaUser>
    {
        List<YogaUser> BackGetPageList(int UserType);
        List<YogaUser> BackGetPageList(string emailOrPhoneOrNickName, int? LoginTimes,
          int? UserType, int? UStatus, int? LoginType, int page, int pagesize, out int count);
        List<YogaUser> GetYogaUserPageList( int Nums);
        List<YogaUser> GetYogaUserXiLian(int Nums);
        List<YogaUser> GetYogaUser_id(int uid);
        List<YogaUser> GetYogaUserType0_id(int uid);
        List<YogaUser> GetYogaUserPageList(int page, int pagesize, out int count);
        YogaUser GetYogaUserById(int id);
        YogaUser GetYogaUserByWechatAuthCode(string WechatAuthCode);
        YogaUser GetAppOrPc(string UserName);
        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        YogaUser CheckUser(string UserName, string Password);
        int updateEntity(YogaUser model);
        YogaUser ExistEmail(string Email);
        YogaUser ExistEmailReg(string strEmail, string NickName);
        YogaUser ExistPhoneReg(string Uphone, string NickName);
        YogaUser ExistValCode(string Valcode);
        YogaUser ExistUphone(string Uphone);

        YogaUser ExistNickName(string NickName);
        List<YogaUser> GetListByNickName(string NickName);
        YogaUser GetYogaUserNickNameIsNotNull(int Uid);
    }
     
}
