using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using IYogaKoo.Client;
using Commons.Helper;
namespace IYogaKoo.Areas.Manage.Controllers
{
    public class BaseController : Controller
    {
        public BasicInfo UserModel { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (UserModel != null)
            {
                UserModel = new BasicInfo();
                //菜单导航
                GetAllMenus();
            }
            else
            {
                //RedirectToAction("LogOn", "Login", new { Area = "Manage" });
            }

            base.Initialize(requestContext);
        }

        protected string hostUrl = "";
        /// <summary>
        /// Action执行前判断
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.checkLogin())
            {
                filterContext.Result = RedirectToRoute("Manage", new { Controller = "BackLogin", Action = "Index", Area = "Manage" });
            }

            base.OnActionExecuting(filterContext);

        }
        /// <summary>
        /// 判断是否登录
        /// </summary>
        protected bool checkLogin()
        {
            string str =Commons.Helper.Security.GetCookie();
            if (String.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string cookieDate = serializer.Serialize(model); //JSON序列化
            UserModel = serializer.Deserialize<BasicInfo>(str);
            if (UserModel == null)
            {
                return false;
            }
            else
            {
                ViewBag.User = UserModel;
            }
            return true;
        }
        /// <summary>
        /// 菜单导航
        /// </summary>
        protected void GetAllMenus()
        {
            YogaMenusServiceClient menusclient = new YogaMenusServiceClient();
            List<ViewYogaMenus> menus = menusclient.GetMenusList();
            ViewBag.Menus = menus;

        }

    }
}
