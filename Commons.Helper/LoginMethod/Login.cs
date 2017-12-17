using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web; 

namespace Commons.Helper
{
    public class Login
    {
         
        static readonly string cookiename_login = "iyoga";
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        public BasicInfo user; 
        /// <summary>
        /// 生成登录成功的cookie等信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bl">记住则保存30天</param>
        public static void CreateLoginInfo(BasicInfo user, bool bl)
        {
            HttpCookie cookie = new System.Web.HttpCookie(cookiename_login);
            
            cookie["Uid"] = user.Uid.ToString();
            cookie["Pwd"] = user.Pwd;
            cookie["NickName"] = AES.Encode(user.NickName, "iyoga");
            cookie["Uphone"] = AES.Encode(user.Uphone, "Uphone");
            cookie["UserType"] = user.UserType.ToString();
            cookie["LoginType"] = user.LoginType.ToString();
            cookie["LastIP"] = user.LastIP.ToString();
            cookie["UEmail"] =AES.Encode(user.UEmail,"UEmail");
            cookie["Url"] = user.Url;
            if (user.Avatar != null)
            {
                cookie["Avatar"] = user.Avatar.ToString();
            }
            else cookie["Avatar"] = "";
            cookie.HttpOnly = false;
            if (bl)
            {
                cookie.Expires = DateTime.Now.AddDays(30);//选择记住则保存30天
                cookie["Expires"] = DateTime.Now.AddDays(30).ToString();
            }
            else
            {
                cookie.Expires = DateTime.Now.AddDays(1);//保存一天
                cookie["Expires"] = DateTime.Now.AddDays(1).ToString();
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns>CurrentUser</returns>
        public static BasicInfo GetCurrentUser()
        {  
            BasicInfo user = new BasicInfo();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename_login];
            if (cookie != null)
            {
                user.Uid = Convert.ToInt32(cookie["Uid"]);
                user.Pwd = cookie["Pwd"];
                user.NickName = AES.Decode(cookie["NickName"],"iyoga");
                user.Uphone = AES.Decode(cookie["Uphone"],"Uphone");
                user.UserType = Convert.ToInt32(cookie["UserType"]);
                user.LoginType = Convert.ToInt32(cookie["LoginType"]);
                user.LastIP = cookie["LastIP"];
                user.UEmail = AES.Decode(cookie["UEmail"],"UEmail");
                user.ValExpire =Convert.ToDateTime(cookie["Expires"]);
                user.Avatar = cookie["Avatar"];
                if (cookie["Url"]!=null)
                    user.Url = cookie["Url"];
            }
            return user;
        }

        /// <summary>
        /// 生成注册成功的cookie等信息
        /// </summary>
        /// <param name="user"></param>
        public static void CreateRegInfo(BasicInfo user)
        {
            HttpCookie cookie = new HttpCookie("Riyoga");
            cookie.Values.Add("Rid", user.Uid.ToString());
            cookie.Values.Add("Rpwd", user.Pwd);
            cookie.HttpOnly = false;
            cookie.Expires = DateTime.Now.AddDays(1);//保存一天
            HttpContext.Current.Response.Cookies.Add(cookie);
        } 

        /// <summary>
        /// 获取当前用户ip
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentIP()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
        /// <summary>
        /// 获取第三方登录的接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IoAuthLogin GetOuathObject(string type)
        {
            IoAuthLogin login = null;
            LoginType loginType = (LoginType)(int.Parse(type));
            switch (loginType)
            {
                case LoginType.Sina微博: login = new Sina(); break;
                case LoginType.QQ: login = new QQ(); break;
                case LoginType.微信: login = new WeiXin(); break;
            
            }
            return login;
        }
        /// <summary>
        /// 获取第三方登录的字符串
        /// </summary>
        /// <returns></returns>
        public static List<string> GetOuathTypes()
        {
            List<string> list = Enum.GetNames(typeof(LoginType)).ToList();
            list.RemoveAt(0);
            list.RemoveAt(0);
            return list;
        }
        /// <summary>
        /// 用名字获取loginType的int值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetLoginType(string name)
        {
            return (int)((LoginType)Enum.Parse(typeof(LoginType), name));
        }
        /// <summary>
        /// 获取url参数这种样式的字符串中每个值
        /// </summary>
        /// <param name="urlParameter"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public static string GetValueFromUrlParameter(string urlParameter, string pName)
        {
            string[] strs = urlParameter.Split('&');
            string result = null;
            foreach (string str in strs)
            {
                string[] p = str.Split('=');
                if (p[0].ToLower() == pName.ToLower())
                    result = p[1];
            }
            return result;
        }
    }
}
