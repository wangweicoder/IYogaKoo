using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using Commons.Helper;
using IYogaKoo.Entity;
using System.Web.Script.Serialization;
namespace IYogaKoo.Areas.Manage.Controllers
{
    public class BackLoginController : Controller
    {
        //
        // GET: /Manage/BackLogin/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int id = 1)
        {
            string userName = Request.Form["username"];
            string userPwd = Request.Form["pwd"];

            using (YogaUserServiceClient client = new YogaUserServiceClient())
            {
                //userPwd = AlipayMD5.GetMD5(userPwd);
                //ViewYogaUser user = client.CheckUser(userName, userPwd);
                //if (user == null)
                //{
                //    return View();
                //}
                //else
                //{
                //保存cookie
                BasicInfo binfo = new BasicInfo();
                //{
                //    Uid = user.Uid,
                //    Pwd = user.Pwd,
                //    UEmail = user.UEmail,
                //    Uphone = user.Uphone,
                //    ValExpire = user.ValExpire,
                //    ValCode = user.ValCode,
                //    LastIP = user.LastIP,
                //    NickName = user.NickName,
                //    LoginTimes = user.LoginTimes,
                //    LoginType = user.LoginType,
                //    UserType = user.UserType

                //};

                if (Checkuser().Equals("1"))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string strUser = js.Serialize(binfo);
                    Security.SaveCookie("manage", strUser);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }

                //更新部分信息
                //user.LoginTimes = user.LoginTimes + 1;
                //user.LastDate = DateTime.Now;
                //user.LastIP = Commons.Helper.Login.GetCurrentIP();
                //client.Update(user);


                // }
            }
        }
        public string Checkuser()
        {
            //return "1";
            string userName = Request.Form["username"];
            string userPwd = Request.Form["pwd"];

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPwd))
            {
                return "0";
            }
            if (userName.Equals("iyogakoo") && userPwd.Equals("iyogakoo666"))
            {
                return "1";
            }
            else
            {
                return "0";
            }
             
            //using (YogaUserServiceClient client = new YogaUserServiceClient())
            //{
            //    userPwd = AlipayMD5.GetMD5(userPwd);
            //    ViewYogaUser user = client.CheckUser(userName, userPwd);
            //    if (user == null)
            //    {
            //        return "0";
            //    }
            //    else
            //    {
            //        return "1";
            //    }
            //}
        }

        public ActionResult LogOut()
        {
            Commons.Helper.Security.SignOut();
            return View("Index");
        }
    }
}
