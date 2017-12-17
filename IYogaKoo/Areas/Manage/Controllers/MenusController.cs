using System;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
namespace IYogaKoo.Areas.Manage.Controllers
{
    public class MenusController : BaseController
    {
        //
        // GET: /Manage/Menus/

        public ActionResult Index(int page = 1)
        {
            List<ViewYogaMenus> list = new List<ViewYogaMenus>();
            int count = 0;
            using (YogaMenusServiceClient client = new YogaMenusServiceClient())
            {
                list = client.GetMenusList(page, 10, out count);
            }
            PagedList<ViewYogaMenus> pagelist = new PagedList<ViewYogaMenus>(list, page, 10, count);
            return View(pagelist);
        }

        //
        // GET: /Manage/Menus/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manage/Menus/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage/Menus/Create

        [HttpPost]
        public ActionResult Create(ViewYogaMenus model)
        {
            try
            {
                // TODO: Add insert logic here
                using (YogaMenusServiceClient client = new YogaMenusServiceClient())
                {
                    client.Add(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/Menus/Edit/5

        public ActionResult Edit(int id)
        {
            ViewYogaMenus menu = new ViewYogaMenus();
            using (YogaMenusServiceClient client = new YogaMenusServiceClient())
            {
                menu = client.GetMenusByid(id);
            }
            return View(menu);
        }

        //
        // POST: /Manage/Menus/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ViewYogaMenus menus)
        {
            try
            {
                // TODO: Add update logic here
                ViewYogaMenus menu = new ViewYogaMenus();
                using (YogaMenusServiceClient client = new YogaMenusServiceClient())
                {
                    menu = client.GetMenusByid(id);
                    menu.name = menus.name;
                    client.Update(menu);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/Menus/Delete/5

        public ActionResult Delete(int id)
        {
            using (YogaMenusServiceClient client = new YogaMenusServiceClient())
            {
                client.DelByid(id);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Manage/Menus/Delete/5

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
