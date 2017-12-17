using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo.Controllers
{
    /// <summary>
    /// 关健字搜索
    /// </summary>
    public class tKeyWordController : Controller
    {
        //
        // GET: /tKeyWord/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /tKeyWord/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /tKeyWord/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /tKeyWord/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /tKeyWord/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /tKeyWord/Edit/5

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
        // GET: /tKeyWord/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /tKeyWord/Delete/5

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
