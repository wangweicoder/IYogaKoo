using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace IYogaKoo
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterView();
            GetCookie();

        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("Server");

            if (Response.Cookies.Count > 0)
            {
                foreach (string s in Response.Cookies.AllKeys)
                {
                    if (s == FormsAuthentication.FormsCookieName || s.ToLower() == "asp.net_sessionid")
                    {
                        Response.Cookies[s].Secure = true;
                    }
                }
            }
        }

        protected void GetCookie()
        {
            //var Request =HttpContext.Current.Request;
            //var Response =HttpContext.Current.Response;
            /* Fix for the Flash Player Cookie bug in Non-IE browsers.
             * Since Flash Player always sends the IE cookies even in FireFox
             * we have to bypass the cookies by sending the values as part of the POST or GET
             * and overwrite the cookies with the passed in values.
             * 
             * The theory is that at this point (BeginRequest) the cookies have not been read by
             * the Session and Authentication logic and if we update the cookies here we'll get our
             * Session and Authentication restored correctly
             */

            try
            {
                string session_param_uid = "Uid";
                string session_param_userType = "UserType";
                // string session_cookie_name = "ASP.NET_SESSIONID";
                string cookiename_login = "iyoga";
                if (HttpContext.Current.Request.Form[session_param_uid] != null)
                {
                    UpdateCookie(cookiename_login, HttpContext.Current.Request.Form[session_param_uid]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_uid] != null)
                {
                    UpdateCookie(cookiename_login, HttpContext.Current.Request.QueryString[session_param_uid]);
                }
                //userType
                if (HttpContext.Current.Request.Form[session_param_userType] != null)
                {
                    UpdateCookie(cookiename_login, HttpContext.Current.Request.Form[session_param_userType]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_userType] != null)
                {
                    UpdateCookie(cookiename_login, HttpContext.Current.Request.QueryString[session_param_userType]);
                }
            }
            catch (Exception)
            {
               // Response.StatusCode = 500;
               // Response.Write("Error Initializing Session");
            }
        }
        void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookie_name);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
        private void RegisterView()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MyViewEngine());

        }

        //public void Application_Error(Object sender, EventArgs e)
        //{
        //    if (HttpContext.Current.IsCustomErrorEnabled)
        //    {
        //        return;
        //    }
        //    var exception = Server.GetLastError();
        //    var httpException = new HttpException(null, exception);
        //    //记录错误日志
        //    if (httpException.InnerException != null)
        //    {
        //        Commons.Helper.CommonInfo.WriteLog(httpException.InnerException.ToString(), DateTime.Now.ToString());
        //    }

        //    var routeDate = new RouteData();
        //    routeDate.Values.Add("controller", "Shared");
        //    routeDate.Values.Add("action", "YogaError");
        //    routeDate.Values.Add("httpException", httpException);
        //    Server.ClearError();
        //    var errorController = ControllerBuilder.Current.GetControllerFactory().CreateController(new RequestContext(new HttpContextWrapper(Context), routeDate), "Error");
        //    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeDate));
            
        //}
        
    }
}