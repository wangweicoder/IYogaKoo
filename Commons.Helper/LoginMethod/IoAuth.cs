
using Commons.Helper.LoginMethod;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Commons.Helper
{
    public interface IoAuthLogin
    {
        /// <summary>
        /// 获取授权页面url
        /// </summary>
        /// <returns></returns>
        string GetLoginUrl(HttpCookieCollection cookies);
        /// <summary>
        /// 获取第三方的用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        OauthInfo GetUser();

        /// <summary>
        /// 判断是否已在网站中注册信息，有则返回用户登录信息
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        ViewYogaUser IsRegister(NameValueCollection collection, HttpCookieCollection cookies);
    }
}
