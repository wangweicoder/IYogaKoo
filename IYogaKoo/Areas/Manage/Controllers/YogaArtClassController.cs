using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    /// <summary>
    /// 类别
    /// </summary>
    public class YogaArtClassController : Controller
    {
        //
        // GET: /Manage/YogaArtClass/

        YogaArtClassServiceClient client;
        List<ViewYogaArtClass> list;
        public YogaArtClassController()
        {
            client = new YogaArtClassServiceClient();
            list = new List<ViewYogaArtClass>();
        }
        public ActionResult Index(int page=1)
        {
            int count = 0; 
            list= client.GetYogaArtClassList(page, 10, out count);

            PagedList<ViewYogaArtClass> pagelist = new PagedList<ViewYogaArtClass>(list,page,10,count);

            return View(pagelist);
        }

        //
        // GET: /Manage/YogaArtClass/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }
        /// <summary>
        /// 获取二级下拉列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetClassNameList(int id)
        {
            list = client.GetYogaArtClassPageList(id);
            return Json(list);
        }
        //
        // GET: /Manage/YogaArtClass/Create

        public ActionResult Create()
        {
            list = client.GetYogaArtClassPageList(0);
            ViewBag.ParentID = new SelectList(list, "ID", "ClassName");
           
            return View();
        }

        //
        // POST: /Manage/YogaArtClass/Create

        [HttpPost]
        public ActionResult Create(ViewYogaArtClass model)
        {
            try
            {
                // TODO: Add insert logic here
                if (!string.IsNullOrEmpty(Request.Form["TwoParentID"]))
                {
                    model.ParentID = Convert.ToInt32(Request.Form["TwoParentID"]);
                }
                else
                {
                    if (model.ParentID == null)
                    {
                        model.ParentID = 0;
                    }
                }
                model.CreateTime = DateTime.Now;
                model.Creator = "admin";
                model.IsDelete = 0;
                client.Add(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/YogaArtClass/Edit/5

        public ActionResult Edit(int id,int parentID)
        {
            list = client.GetYogaArtClassPageList(0);

            ViewYogaArtClass model = client.GetById(id);
            ViewYogaArtClass modelUp = client.GetById(model.ParentID.Value);
            if (modelUp != null)
            {
                if (modelUp.ParentID != 0)
                {
                    //三级
                    ViewYogaArtClass modelthree = client.GetById(model.ParentID.Value);
                    ViewBag.TopParentID = new SelectList(list, "ID", "ClassName", modelthree.ParentID);
                }
                else
                {
                    //二级
                    ViewBag.TopParentID = new SelectList(list, "ID", "ClassName", modelUp.ID);
                }
            }
            else
            {
                //二级
                ViewBag.TopParentID = new SelectList(list, "ID", "ClassName", model.ID);
            }
            
            return View(model);
        }

        //
        // POST: /Manage/YogaArtClass/Edit/5

        [HttpPost]
        public ActionResult Edit(ViewYogaArtClass model)
        {
            try
            {
                // TODO: Add update logic here
                int ParentID = 0;

                if (!string.IsNullOrEmpty(Request.Form["hidTwoid"]))
                {
                    ParentID = Convert.ToInt32(Request.Form["hidTwoid"]);
                }
                else 
                {
                    if (!string.IsNullOrEmpty(Request.Form["hidTopid"]))
                    {
                        ParentID = Convert.ToInt32(Request.Form["hidTopid"]);
                    }
                }
                ViewYogaArtClass entity = client.GetById(model.ID);
                if (entity != null)
                {
                    model.ID = entity.ID;
                    model.ParentID = ParentID;
                    model.CreateTime = entity.CreateTime;
                    model.Creator = entity.Creator;
                    model.IsDelete = entity.IsDelete;
                    client.Update(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
 
        //
        // POST: /Manage/YogaArtClass/Delete/5

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                list = client.GetYogaArtClassPageList(id);

                if (list.Count() > 0)
                {
                    return Json(new { code = 2 });//有子项
                }
                else {
                    client.Delete(id.ToString());
                    return Json(new { code = 0 });
                }
               
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
    }
}
