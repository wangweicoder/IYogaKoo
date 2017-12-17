using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service.Interfaces
{
    
    public interface IYogaUserService
    {
        List<ViewYogaUser> BackGetPageList(int UserType);
        List<ViewYogaUser> BackGetPageList(string emailOrPhoneOrNickName, int? LoginTimes,
       int? UserType, int? UStatus, int? LoginType, int page, int pagesize, out int count);

        List<ViewYogaUser> GetYogaUserPageList( int page, int pagesize, out int count);
        /// <summary>
        /// 瑜伽达人/导师
        /// </summary>
        /// <param name="Nums"></param>
        /// <returns></returns>
        List<ViewYogaUser> GetYogaUserPageList(int Nums);
        List<ViewYogaUser> GetYogaUser_id(int uid);
        List<ViewYogaUser> GetYogaUserType0_id(int uid);
        /// <summary>
        /// 瑜伽习练者
        /// </summary>
        /// <param name="Nums"></param>
        /// <returns></returns>
        List<ViewYogaUser> GetYogaUserXiLian(int Nums);

        ViewYogaUser GetYogaUserById(int id);
        ViewYogaUser GetYogaUserByWechatAuthCode(string WechatAuthCode);
        ViewYogaUser CheckUser(string UserName, string Password);
        ViewYogaUser GetAppOrPc(string UserName);
        int Add(ViewYogaUser model);
        ViewYogaUser Return_AddUid(ViewYogaUser model);
        ViewYogaUser GetById(int id);

        int Update(ViewYogaUser model);

        int Delete(string deletelist);

        ViewYogaUser ExistEmailReg(string strEmail, string NickName);
        ViewYogaUser ExistPhoneReg(string Uphone, string NickName); 
        /// <summary>
        /// 从邮箱里取valcode判断表中是否存在
        /// </summary>
        /// <param name="Valcode"></param>
        /// <returns></returns>
        ViewYogaUser ExistValCode(string Valcode);        
        ViewYogaUser ExistUphone(string Uphone);
        ViewYogaUser ExistEmail(string Email);

        ViewYogaUser ExistNickName(string NickName);

        ViewYogaUser GetYogaUserNickNameIsNotNull(int Uid);
        
    }
}
