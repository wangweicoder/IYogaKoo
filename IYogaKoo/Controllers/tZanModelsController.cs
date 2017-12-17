using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo.Controllers
{
    public class tZanModelsController : Controller
    {
        //
        // GET: /tZanModels/
        ///获取用户信息cookie
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();

        tZanModelsServiceClient client;
        List<ViewtZanModels> list;
        method method;
        public tZanModelsController()
        {
            ViewBag.user = user;
            client = new tZanModelsServiceClient();
            list = new List<ViewtZanModels>();
            method = new Commons.Helper.method();
            #region  站内信-信息数量

            int tinstatcount = 0;
            int follcount = 0;
            int zancount = 0;
            int msgcount = 0;

            method.InstationInfo(user.Uid, out   tinstatcount, out   follcount, out   zancount, out   msgcount);

            ViewBag.tinstatcount = tinstatcount;
            ViewBag.follcount = follcount;
            ViewBag.zancount = zancount;
            ViewBag.msgcount = msgcount;
            ViewBag.AllCount = tinstatcount + follcount + zancount + msgcount;
            #endregion
        }
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /tZanModels/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /tZanModels/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /tZanModels/Create
        /// <summary>
        /// 推荐导师时的赞
        /// </summary>
        [HttpPost]
        public ActionResult Create(int? id)
        {
            try
            {
                int resultcode = 0;
                int Uid = Convert.ToInt32(Request.Form["uid"]);//专页的导师id
                ViewtZanModels model = client.GetByFromToUid(Uid,user.Uid,1);
                if (model == null)
                {
                    model = new ViewtZanModels();
                    model.iToUid = Uid;
                    model.iFromUid = user.Uid;//登录者id
                    model.CreateDate = DateTime.Now;
                    model.iType = user.UserType;
                    resultcode = client.Add(model);
                }
                else
                {
                    return Json(new { code = 2 });//已经点过赞的
                }                
                return Json(new { code = resultcode });
            }
            catch
            {
                return Json(new { code = 1 });
            }            
        }

        //
        // GET: /tZanModels/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /tZanModels/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /tZanModels/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /tZanModels/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
