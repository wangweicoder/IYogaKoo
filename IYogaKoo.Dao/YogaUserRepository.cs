using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Dao
{

    public class YogaUserRepository : Repository<YogaUser>, IYogaUserRepository
    {
        /// <summary>
        /// 后台查询全部用户
        /// </summary>
        /// <param name="Nums"></param>
        /// <returns></returns>
        public List<YogaUser> BackGetPageList(int UserType)
        { 
            return dbSet.Where(x => x.delState == 0 && x.UserType == UserType).OrderByDescending(a => a.RegDate).ToList(); 
        }
        //后台start 
        public List<YogaUser> BackGetPageList(string emailOrPhoneOrNickName, int? LoginTimes,
         int? UserType, int? UStatus, int? LoginType, int page, int pagesize, out int count)
        {
            IQueryable<YogaUser> linq = dbSet.OrderBy(x => x.RegDate);
            if (!string.IsNullOrEmpty(emailOrPhoneOrNickName))
            {
                linq = linq.Where(x => x.UEmail.Contains(emailOrPhoneOrNickName) || x.Uphone.Contains(emailOrPhoneOrNickName) || x.NickName.Contains(emailOrPhoneOrNickName));
            }
             
            if (LoginTimes != null)
            {
                linq = linq.Where(x => x.LoginTimes == LoginTimes.Value);
            }
            if (UserType != null)
            {
                linq = linq.Where(x => x.UserType == UserType.Value);
            }
            if (UStatus != null)
            {
                linq = linq.Where(x => x.UStatus == UStatus.Value);
            }
            if (LoginType != null)
            {
                linq = linq.Where(x => x.LoginType == LoginType.Value);
            }

            count = linq.Count();

            return linq.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }

        //end
        public List<YogaUser> GetYogaUserPageList(int Nums)
        {
            if (Nums != 0)
            {
                return dbSet.OrderByDescending(a => a.RegDate).Take(Nums).ToList();
            }
            else
            {
                return dbSet.Where(x=>!string.IsNullOrEmpty(x.NickName)).OrderByDescending(a => a.RegDate).ToList();
            }
        }
        public List<YogaUser> GetYogaUserXiLian(int Nums)
        {
            if (Nums != 0)
            {
                return dbSet.OrderByDescending(a => a.RegDate).Where(a => a.UserType == (int)UserType.瑜伽会员).Take(Nums).ToList();
            }
            else
            {
                return dbSet.OrderByDescending(a => a.RegDate).Where(a => a.UserType == (int)UserType.瑜伽会员).ToList();
            }
        }
        /// <summary>
        /// 根据Uid ，UserType获取YogaUser 瑜伽导师 列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<YogaUser> GetYogaUser_id(int uid)
        { 
            return dbSet.Where(a => a.UserType == (int)UserType.瑜伽导师 && a.Uid==uid).ToList();
            
        }
        /// <summary>
        /// 根据Uid ，UserType获取YogaUser 瑜伽会员列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<YogaUser> GetYogaUserType0_id(int uid)
        {
            return dbSet.Where(a => a.UserType == (int)UserType.瑜伽会员 && a.Uid == uid).ToList();

        }
        public List<YogaUser> GetYogaUserPageList(int page, int pagesize, out int count)
        {
            count = dbSet.Count();

            return dbSet.OrderByDescending(a => a.RegDate).Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
        /// <summary>
        /// 根据Uid 获取YogaUser 信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YogaUser GetYogaUserById(int id)
        {
            return dbSet.Where(a => a.Uid == id).FirstOrDefault();
        }
       
        /// <summary>
        ///  判断是APP/PC端的用户登录
        /// </summary>
        /// <param name="UserName">邮箱/电话</param>
        /// <returns>App =not null ; pc = null</returns>
        public YogaUser GetAppOrPc(string UserName)
        {
            IQueryable<YogaUser> query = dbSet.Where(a => a.UEmail == UserName && a.UStatus == (int)Ustatus.开启 && a.LoginType == 1 && a.InputType == 1 || (a.Uphone == UserName && a.UStatus == (int)Ustatus.开启 && a.LoginType == 1 && a.InputType == 1));
            
            return query.FirstOrDefault();
        }
        /// <summary>
        /// 判断用户与密码/手机与密码是否存在
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public YogaUser CheckUser(string UserName, string Password)
        {
            return dbSet.Where(a => a.UEmail == UserName && a.Pwd == Password && a.UStatus == (int)Ustatus.开启 || (a.Uphone == UserName && a.Pwd == Password && a.UStatus == (int)Ustatus.开启)).FirstOrDefault();
        }
        /// <summary>
        /// 判断‘微信登录编号’是否存在  
        /// </summary>
        /// <param name="WechatAuthCode"></param>
        /// <returns></returns>
        public YogaUser GetYogaUserByWechatAuthCode(string WechatAuthCode)
        {
            return dbSet.Where(a => a.WechatAuthCode == WechatAuthCode).FirstOrDefault();
        }
        public int updateEntity(YogaUser model)
        { 
            var entity = dbSet.Find(model.Uid);

            if (entity != null)
            {
                Context.Entry(entity).State = System.Data.EntityState.Detached;
                //这个是在同一个上下文能修改的关键 
            }

            entity = model;

            Update(entity);

            return Save();


        }

        public YogaUser ExistEmail(string Email)
        {
            return dbSet.Where(a => a.UEmail == Email).FirstOrDefault();
        }
        /// <summary>
        /// 判断‘Email及昵称’是否存在  
        /// </summary>
        /// <param name="strEmail"></param>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public YogaUser ExistEmailReg(string strEmail, string NickName)
        { 
            return dbSet.Where(a => a.UEmail == strEmail && a.NickName == NickName).FirstOrDefault();
        }
        /// <summary>
        /// 判断‘电话及昵称’是否存在  
        /// </summary>
        /// <param name="Uphone"></param>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public YogaUser ExistPhoneReg(string Uphone, string NickName )
        {
            return dbSet.Where(a => a.Uphone == Uphone && a.NickName == NickName ).FirstOrDefault();
        }
        /// <summary>
        ///  判断‘验证码’是否存在  
        /// </summary>
        /// <param name="Valcode"></param>
        /// <returns></returns>
        public YogaUser ExistValCode(string Valcode)
        {
            return dbSet.Where(a => a.ValCode == Valcode ).FirstOrDefault();
        }        
        /// <summary>
        /// 判断‘手机号’是否存在  
        /// </summary>
        /// <param name="Uphone"></param>
        /// <returns></returns>
        public YogaUser ExistUphone(string Uphone)
        { 
            return dbSet.Where(a => a.Uphone == Uphone).FirstOrDefault();
        }
        /// <summary>
        /// 判断‘昵称’是否存在  
        /// </summary>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public YogaUser ExistNickName(string NickName)
        { 
            return dbSet.Where(a => a.NickName == NickName).FirstOrDefault();
        }

        public YogaUser GetYogaUserNickNameIsNotNull(int Uid)
        {
            return dbSet.Where(a => a.Uid == Uid && !string.IsNullOrEmpty(a.NickName)).FirstOrDefault();
        }

        //查询
        public List<YogaUser> GetListByNickName(string NickName)
        {
            return dbSet.Where(u => u.delState == null && u.NickName.Contains(NickName) && u.UserType == 0).ToList();
        }

    }
}
