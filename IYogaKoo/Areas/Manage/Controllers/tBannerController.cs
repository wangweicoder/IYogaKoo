using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class tBannerController : Controller
    {
        //
        // GET: /Manage/tBanner/

        tBannerServiceClient client;
        List<ViewtBanner> list;
        public tBannerController()
        {
            client = new tBannerServiceClient();
            list = new List<ViewtBanner>();
        }
        public ActionResult Index(int page=1)
        {
            int count = 0;
            list=client.GettBannerPageList(page, 10, out count);
            PagedList<ViewtBanner> pagelist = new PagedList<ViewtBanner>(list, page, 10, count);

            return View(pagelist);
        }

        //
        // GET: /Manage/tBanner/Details/5

        public ActionResult Details(int id)
        {
            ViewtBanner model = client.GettBannerById(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /Manage/tBanner/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage/tBanner/Create

        [HttpPost]
        public ActionResult Create(ViewtBanner model)
        {
            try
            {
                // TODO: Add insert logic here

                model.CreateDate = DateTime.Now;
                client.Add(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/tBanner/Edit/5

        public ActionResult Edit(int id)
        {
           ViewtBanner model= client.GettBannerById(id);
           return View(model);
        }

        //
        // POST: /Manage/tBanner/Edit/5

        [HttpPost]
        public ActionResult Edit(ViewtBanner model)
        {
            try
            { 
                // TODO: Add update logic here
                int iType = model.iType.Value;
                string strPic = model.spic;
                string strUrl = model.sUrl;
                model = client.GettBannerById(model.ID);
                if (Directory.Exists(model.spic))
                {
                    Directory.Delete(model.spic);//删除文件夹中图片
                }
                ViewtBanner entity = new ViewtBanner();
                entity.ID = model.ID;
                entity.spic = strPic;
                entity.sUrl = strUrl;
                entity.iType = iType;
                entity.CreateDate = DateTime.Now;
                client.Update(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/tBanner/Delete/5

        public ActionResult Delete(int id,string spic)
        {
            client.Delete(id.ToString());
            if (Directory.Exists(spic))
            {
                Directory.Delete(spic);//删除文件夹中图片
            } 
            return RedirectToAction("Index");
        }

        //
        // POST: /Manage/tBanner/Delete/5

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
