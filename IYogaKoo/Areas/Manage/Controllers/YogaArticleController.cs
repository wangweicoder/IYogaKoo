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
    /// 文章
    /// </summary>
    public class YogaArticleController : Controller
    {
        //
        // GET: /Manage/YogaArticle/
        YogaArticleServiceClient client;
        YogaArtClassServiceClient clientClass;
        List<ViewYogaArticle> list;
        public YogaArticleController()
        {
            client = new YogaArticleServiceClient();
            clientClass = new YogaArtClassServiceClient();
            list = new List<ViewYogaArticle>();
        }
        public ActionResult Index(int page=1)
        {
            int count=0;
            list = client.GetYogaArticleList(page, 10, out count);

            PagedList<ViewYogaArticle> pagelist = new PagedList<ViewYogaArticle>(list,page,10,count);

            List<ViewArticleClassGroup> listGroup = new List<ViewArticleClassGroup>();
            foreach (var item in list)
            {
                ViewArticleClassGroup model = new ViewArticleClassGroup();
                model.artice = item;

                model.artclass = clientClass.GetById(item.ClassID);
                listGroup.Add(model);
            }
            ViewBag.listGroup = listGroup;

            return View(pagelist);
        }

        //
        // GET: /Manage/YogaArticle/Details/5

        public ActionResult Details(int id)
        {
           ViewYogaArticle model= client.GetById(id);
           ViewYogaArtClass entity = clientClass.GetById(model.ClassID);
           if (entity != null)
           {
               if (entity.ParentID != 0)
               { 
                   ViewYogaArtClass entity2 = clientClass.GetById(entity.ParentID.Value);
                   if (entity2.ParentID != 0)
                   {
                       ViewYogaArtClass entity3 = clientClass.GetById(entity2.ParentID.Value);
                        
                       ViewBag.ClassName = entity3.ClassName;
                        
                   }
                   else
                   {
                       ViewBag.ClassName = entity2.ClassName;
                   }
               }
               else
               {
                   ViewBag.ClassName = entity.ClassName;
               }
           }
           return View(model);
        }

        //
        // GET: /Manage/YogaArticle/Create

        public ActionResult Create()
        { 
            List<ViewYogaArtClass> listClass = clientClass.GetYogaArtClassPageList(0);
            ViewBag.ClassID = new SelectList(listClass, "ID", "ClassName");
            return View();
        }

        //
        // POST: /Manage/YogaArticle/Create

        [HttpPost,ValidateInput(false)]
        public ActionResult Create(ViewYogaArticle model)
        {
            try
            {
                // TODO: Add insert logic here
                if (!string.IsNullOrEmpty(Request.Form["TwoParentID"]))
                {
                    model.ClassID = Convert.ToInt32(Request.Form["TwoParentID"]);
                }
                else
                { 
                   if(!string.IsNullOrEmpty(Request.Form["ClassID"]))
                   model.ClassID = Convert.ToInt32(Request.Form["ClassID"]); 
                }
                model.CreateTime = DateTime.Now;
                model.Creator = "admin";
                model.IsDelete = 0;
                model.IsPicture = 0;
                model.IsCheck = 0;
                model.Uid = 0;
                model.CheckID = 0;
                client.Add(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/YogaArticle/Edit/5

        public ActionResult Edit(int id)
        {
            ViewYogaArticle model = client.GetById(id);
            ViewBag.ClassName = clientClass.GetById(model.ClassID).ClassName;

            return View(model);
        }
        //
        // POST: /Manage/YogaArticle/Edit/5

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(ViewYogaArticle model)
        {
            try
            {
                // TODO: Add update logic here
                ViewYogaArticle entity = client.GetById(model.ID);
                  
                entity.ArticleCon = model.ArticleCon;
                entity.ArticleTitle = model.ArticleTitle;
                entity.Author = model.Author;
                entity.PicPath = model.PicPath;

                client.Update(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
 
        //
        // POST: /Manage/YogaArticle/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add update logic here
                ViewYogaArticle model = client.GetById(id);
                
                model.IsDelete = 1;
                client.Update(model);
                
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
    }
}
