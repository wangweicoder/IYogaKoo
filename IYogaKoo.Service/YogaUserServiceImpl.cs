using IYogaKoo.Dao.Interfaces;
using IYogaKoo.Entity;
using IYogaKoo.Service.Interfaces;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.Service
{
    public class YogaUserServiceImpl : IYogaUserService
    {
        IYogaUserRepository Repository;
        public YogaUserServiceImpl(IYogaUserRepository Repository)
        {
            this.Repository = Repository;
        }
        public List<ViewYogaUser> BackGetPageList(int UserType)
        {
            List<YogaUser> list = Repository.BackGetPageList(UserType);

            List<ViewYogaUser> model = new List<ViewYogaUser>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUser.ToViewModel(item));
            }
            return model;
        }
        //后台start
        public List<ViewYogaUser> BackGetPageList(string emailOrPhoneOrNickName, int? LoginTimes,
         int? UserType, int? UStatus, int? LoginType, int page, int pagesize, out int count)
        {
            List<YogaUser> list = Repository.BackGetPageList(emailOrPhoneOrNickName, LoginTimes, UserType, UStatus, LoginType, page, pagesize, out  count);

            List<ViewYogaUser> model = new List<ViewYogaUser>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUser.ToViewModel(item));
            }
            return model;
        }
        //end
        public List<ViewYogaUser> GetYogaUserPageList(int Nums)
        {
            List<YogaUser> list = Repository.GetYogaUserPageList(Nums);

            List<ViewYogaUser> model = new List<ViewYogaUser>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUser.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaUser> GetYogaUser_id(int uid)
        {
            List<YogaUser> list = Repository.GetYogaUser_id(uid);

            List<ViewYogaUser> model = new List<ViewYogaUser>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUser.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogaUser> GetYogaUserType0_id(int uid)
        {
            List<YogaUser> list = Repository.GetYogaUserType0_id(uid);

            List<ViewYogaUser> model = new List<ViewYogaUser>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUser.ToViewModel(item));
            }
            return model;
        }
        public List<ViewYogaUser> GetYogaUserXiLian(int Nums)
        {
            List<YogaUser> list = Repository.GetYogaUserPageList(Nums);

            List<ViewYogaUser> model = new List<ViewYogaUser>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUser.ToViewModel(item));
            }
            return model;
        }

        public List<ViewYogaUser> GetYogaUserPageList(int page, int pagesize, out int count)
        {
            List<YogaUser> list = Repository.GetYogaUserPageList(page, pagesize, out count);

            List<ViewYogaUser> model = new List<ViewYogaUser>();

            foreach (var item in list)
            {
                model.Add(ViewYogaUser.ToViewModel(item));
            }
            return model;
        }

        public int Add(ViewYogaUser model)
        {
            Repository.Add(ViewYogaUser.ToEntity(model));
            return Repository.Save();
        }
        public ViewYogaUser Return_AddUid(ViewYogaUser model)
        {
            YogaUser user = Repository.Add(ViewYogaUser.ToEntity(model));
            Repository.Save();
            return ViewYogaUser.ToViewModel(user);
        }
        public ViewYogaUser GetById(int id)
        {
            return ViewYogaUser.ToViewModel(Repository.Get(id));
        }

        public int Update(ViewYogaUser model)
        {
            Repository.updateEntity(ViewYogaUser.ToEntity(model));
            return Repository.Save();
        }

        public int Delete(string deletelist)
        {
            string[] list = deletelist.TrimEnd(',').Split(',');
            foreach (var item in list)
            {
                Repository.Delete(Repository.Get(int.Parse(item)));
            }
            return Repository.Save();
        }
        public ViewYogaUser GetAppOrPc(string UserName)
        {
            return ViewYogaUser.ToViewModel(Repository.GetAppOrPc(UserName));
        }
        public ViewYogaUser CheckUser(string UserName, string Password)
        {
            return ViewYogaUser.ToViewModel(Repository.CheckUser(UserName, Password));
        }
        public ViewYogaUser GetYogaUserById(int id)
        {
            return ViewYogaUser.ToViewModel(Repository.GetYogaUserById(id));
        }

        public ViewYogaUser GetYogaUserByWechatAuthCode(string WechatAuthCode)
        {
            return ViewYogaUser.ToViewModel(Repository.GetYogaUserByWechatAuthCode(WechatAuthCode));
        }
        public ViewYogaUser ExistEmail(string Email)
        {
            return ViewYogaUser.ToViewModel(Repository.ExistEmail(Email));
        }
        public ViewYogaUser ExistEmailReg(string strEmail, string NickName)
        {
            return ViewYogaUser.ToViewModel(Repository.ExistEmailReg(strEmail, NickName));
        }
        public ViewYogaUser ExistPhoneReg(string Uphone, string NickName)
        {
            return ViewYogaUser.ToViewModel(Repository.ExistPhoneReg(Uphone, NickName));
        }

        public ViewYogaUser ExistValCode(string Valcode)
        {
            return ViewYogaUser.ToViewModel(Repository.ExistValCode(Valcode));
        }
        public ViewYogaUser ExistUphone(string Uphone)
        {
            return ViewYogaUser.ToViewModel(Repository.ExistUphone(Uphone));
        }
        public ViewYogaUser ExistNickName(string NickName)
        {
            return ViewYogaUser.ToViewModel(Repository.ExistNickName(NickName));
        }

        public ViewYogaUser GetYogaUserNickNameIsNotNull(int Uid)
        {
            return ViewYogaUser.ToViewModel(Repository.GetYogaUserNickNameIsNotNull(Uid));
        }
    }
}
