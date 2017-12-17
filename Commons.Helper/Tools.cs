using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Commons.Helper
{
    public  class Tools
    {
        static readonly string cookiename_login = "iyogaUrl";
       
        #region 获取网站路径
        /// <summary>
        /// 获取根目录Url (mc add [2010-3-30])
        /// </summary>
        /// <returns>string</returns>
        public static string GetRootUrl()
        {
            string strHttp = HttpContext.Current.Request.Url.AbsoluteUri;
            int i = strHttp.IndexOf('/');
            string strWebSiteUrl = strHttp.Substring(0, i + 2) + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
            return strWebSiteUrl.Trim('/');
        }
        /// <summary>
        /// 得到网站的根目录
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    return HttpContext.Current.Request.ApplicationPath;
                }
                return HttpContext.Current.Request.ApplicationPath + "/";
            }
        }
        #endregion
        public static void cookieUrl(string url)
        {
            if (url != "" && url != null)
            {
                //if (url.IndexOf(".aspx") > 1)
                //{ 
                //    url = url.Replace(".aspx","");
                //}
                //if (url.IndexOf("?id=") > 1)
                //{
                //    url = url.Replace("?id=", "/");
                //}


                HttpCookie cookie = new System.Web.HttpCookie(cookiename_login);
                cookie["Url"] = url;
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
        /// <summary>
        /// 用于将错误信息输出到txt文件
        /// </summary> 
        /// <param name="actionName">控制器动作名称</param>
        /// <param name="errorMessage">错误详细信息</param>
        public static void WriteTextLog(string actionName, string errorMessage)
        {
            try
            {
                string path = "~/App_Data/error_log" + DateTime.Today.ToString("yyMMdd") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\\动作："+actionName+" : ");
                    w.WriteLine("{0}", DateTime.Now); 
                    w.WriteLine(errorMessage);
                    w.WriteLine("________________________________________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                WriteTextLog(actionName,ex.Message);
            }
        }

    }
 
}
