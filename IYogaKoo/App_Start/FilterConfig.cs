using System;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
    public class AppHandleErrorAttribute : HandleErrorAttribute
    {        
        public override void  OnException(ExceptionContext filterContext)
        {
            Exception Error= filterContext.Exception;
            string Message = Error.Message;
            string Url = HttpContext.Current.Request.RawUrl;
            Commons.Helper.CommonInfo.WriteLog( Url,Message,DateTime.Now.Day.ToString());
            base.OnException(filterContext);
        }
    }
}