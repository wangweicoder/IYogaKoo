using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class BacktLearingController : Controller
    {
        //qiqi 社区
        // GET: /Manage/BacktLearing/
        tLearingServiceClient client;
        ViewtLearing model;
        List<ViewtLearing> list;
        public BacktLearingController()
        {
            client = new tLearingServiceClient();
            model = new ViewtLearing();
            list = new List<ViewtLearing>();

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            
            return View();
        }
    }
}
