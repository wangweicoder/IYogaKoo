using Commons.Helper;
using Commons.Helper.LoginMethod;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using IYogaKoo.ViewModel.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Commons.Helper.WebHelper;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class ActivityController : BaseController
    {
        //
        // GET: /Manage/Class/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SearchActivity(int page = 1, int size = 10)
        {
            ClassServiceClient client = new ClassServiceClient();
            PageResult<ViewClass> pr = client.Classes(page, size);
            return Json(pr, JsonRequestBehavior.AllowGet);
        }
    }
}
