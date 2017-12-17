using IYogaKoo.Dao;
using IYogaKoo.Service;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Client
{

    public class YogaUserServiceClient : IYogaUserService, IDisposable
    {
        public IYogaUserService YogaUserServiceImpl { get; set; }
         
         public YogaUserServiceClient()
        {
            YogaUserServiceImpl = new YogaUserServiceImpl(new YogaUserRepository());
        }
         
        

        //后台 start  
         public List<ViewYogaUser> BackGetPageList(int UserType)
         {
             try
             {
                 return YogaUserServiceImpl.BackGetPageList(UserType);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         public List<ViewYogaUser> BackGetPageList(string emailOrPhoneOrNickName, int? LoginTimes,
             int? UserType, int? UStatus, int? LoginType, int page, int pagesize, out int count)
        {
            try
            {
                return YogaUserServiceImpl.BackGetPageList(emailOrPhoneOrNickName, LoginTimes,
              UserType, UStatus,  LoginType,page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //end
         public List<ViewYogaUser> GetYogaUserPageList(int Nums)
         {
             try
             {
                 return YogaUserServiceImpl.GetYogaUserPageList(Nums);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         public List<ViewYogaUser> GetYogaUser_id(int uid)
        {
             try
             {
                 return YogaUserServiceImpl.GetYogaUser_id(uid);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         public List<ViewYogaUser> GetYogaUserType0_id(int uid)
         {
             try
             {
                 return YogaUserServiceImpl.GetYogaUserType0_id(uid);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         public List<ViewYogaUser> GetYogaUserXiLian(int Nums)
         {
             try
             {
                 return YogaUserServiceImpl.GetYogaUserXiLian(Nums);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
        
         public List<ViewYogaUser> GetYogaUserPageList( int page, int pagesize, out int count)
        {
            try
            {
                return YogaUserServiceImpl.GetYogaUserPageList( page, pagesize, out count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

         public int Add(ViewYogaUser model)
        {
            try
            {
                return YogaUserServiceImpl.Add(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 添加完成,取uid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         public ViewYogaUser Return_AddUid(ViewYogaUser model)
         {
             try
             {
                 return YogaUserServiceImpl.Return_AddUid(model);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         public ViewYogaUser GetById(int id)
        {
            try
            {
                return YogaUserServiceImpl.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         /// <summary>
         ///  判断是APP/PC端的用户登录(App =notnull ; pc = null)
         /// </summary>
         /// <param name="UserName">邮箱/电话</param>
         /// <returns>App =notnull ; pc = null</returns>
         public ViewYogaUser GetAppOrPc(string UserName)
         {
             try
             {
                 return YogaUserServiceImpl.GetAppOrPc(UserName);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public ViewYogaUser CheckUser(string UserName, string Password)
         {
             try
             {
                 return YogaUserServiceImpl.CheckUser(UserName, Password);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public int Update(ViewYogaUser model)
        {
            try
            {
                return YogaUserServiceImpl.Update(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(string deletelist)
        {
            try
            {
                return YogaUserServiceImpl.Delete(deletelist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
        public List<ViewYogaUser> GetogaUserGroupList(int page, int pagesize, out int count)
        {
            try
            {
                List<ViewYogaUser> model = new List<ViewYogaUser>();

                List<ViewYogaUser> list = GetYogaUserPageList(page, pagesize, out count);

                //var questionList = from u in list
                //                   where u.ID == null
                //                   select u;


                //var answerList = from u in list
                //                 where u.ID != null
                //                 select u;


                //foreach (var item in questionList)
                //{
                //    tEducationModel temp = new tEducationModel();

                //    //temp.question = item;

                //    //temp.answer = (from u in answerList
                //    //               where u.ID == item.ID
                //    //               select u).FirstOrDefault();

                //    //ViewModelProduct product = new ViewModelProduct();

                //    //using (ProductServiceClient client = new ProductServiceClient())
                //    //{
                //    //    product = client.GetById((int)item.ProductId);
                //    //}
                //    //temp.product = product;
                //    model.Add(temp);
                //}
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void Dispose()
        {

        }
         
        public ViewYogaUser GetYogaUserById(int id)
        {
            try
            {
                return YogaUserServiceImpl.GetYogaUserById(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public ViewYogaUser GetYogaUserByWechatAuthCode(string WechatAuthCode)
        {
            try
            {
                return YogaUserServiceImpl.GetYogaUserByWechatAuthCode(WechatAuthCode);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //检查唯一信息是有否重复 
        public ViewYogaUser ExistEmailReg(string strEmail, string NickName)
        {
            try
            {
                return YogaUserServiceImpl.ExistEmailReg( strEmail, NickName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ViewYogaUser ExistPhoneReg(string Uphone, string NickName)
        {
            try
            {
                return YogaUserServiceImpl.ExistPhoneReg(Uphone, NickName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ViewYogaUser ExistValCode(string Valcode)
        {
            try
            {
                return YogaUserServiceImpl.ExistValCode(Valcode);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
       
        public ViewYogaUser ExistUphone(string Uphone)
        {
            try
            {
                return YogaUserServiceImpl.ExistUphone(Uphone);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ViewYogaUser ExistEmail(string Email)
        {
            try
            {
                return YogaUserServiceImpl.ExistEmail(Email);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ViewYogaUser ExistNickName(string NickName)
        {
            try
            {
                return YogaUserServiceImpl.ExistNickName(NickName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ViewYogaUser GetYogaUserNickNameIsNotNull(int Uid)
        {
            try
            {
                return YogaUserServiceImpl.GetYogaUserNickNameIsNotNull(Uid);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
