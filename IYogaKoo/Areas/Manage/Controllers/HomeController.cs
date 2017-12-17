using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Manage/Home/

        public ActionResult Index()
        {
            return View();
        }

    }

}
