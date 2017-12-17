using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class tQuestionController : Controller
    {
        method method;
        public tQuestionController()
        {
            method = new method();
        }

        public ActionResult Index(int page = 1)
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            ViewBag.Diclist = DicItemlist;
            tQuestionServiceClient client = new tQuestionServiceClient();
            string whereStr = "";

            whereStr += "IsFAQ!" + true + ",";
            whereStr += "IsDelete!" + false + ",";
            whereStr += "BeFrom!" + 2 + ",";
            int pagesize = 120;
            int count = 0;
            var list = client.GetList(whereStr, page, pagesize, out count);
            return View(list);
        }
        public ActionResult UserQuestionIndex(int page = 1)
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            ViewBag.Diclist = DicItemlist;
            tQuestionServiceClient client = new tQuestionServiceClient();
            string whereStr = "";

            whereStr += "IsFAQ!" + false + ",";
            whereStr += "IsDelete!" + false + ",";
            whereStr += "BeFrom!" + 1 + ",";
            int pagesize = 12;
            int count = 0;
            List<ViewtQuestion> list = client.GetList(whereStr, page, pagesize, out count).OrderByDescending(p=>p.QuestionTime).ToList();
            Webdiyer.WebControls.Mvc.PagedList<ViewtQuestion> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewtQuestion>(list, page, pagesize, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("UserQuestionIndexList", pagelist);
            }
            return View(pagelist);
        }

        public ActionResult AddFAQ()
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            ViewBag.Diclist = DicItemlist;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddFAQ(FormCollection collection)
        {
            //ViewtQuestion model
            ViewtQuestion model = new ViewtQuestion();
            tQuestionServiceClient client = new tQuestionServiceClient();
            model.Uid = int.Parse(ConfigurationManager.AppSettings["BACK_POSTER"]);
            model.QuestionTime = DateTime.Now;
            model.BeFrom = 2;
            model.IsFAQ = true;
            model.QuestionContent = collection["QuestionContent"];
            model.ReplyContent = collection["ReplyContent"];
            model.TitleID = int.Parse(collection["iType"]);
            if (collection["ReplyUid"] != null)
                model.ReplyUid = int.Parse(collection["ReplyUid"]);
            if (collection["Hot"].Contains("true"))
                model.Hot = true;
            else
                model.Hot = false;
            try
            {
                client.Add(model);
                return Json(new { code = 0 });
            }
            catch (Exception e)
            {
                return Json(new { code = 1 });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Delete(int id)
        {
            try
            {
                tQuestionServiceClient client = new tQuestionServiceClient();
                ViewtQuestion model = client.GetById(id);
                model.IsDelete = true;
                client.Edit(model);
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        public ActionResult Update(int id)
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            //ViewBag.Diclist = DicItemlist;
            SelectList Diclist = new SelectList(DicItemlist, "ID", "ItemName");
            ViewBag.Diclist = Diclist;


            tQuestionServiceClient client = new tQuestionServiceClient();
            ViewtQuestion model = client.GetById(id);
            ViewBag.Model = model;
            ViewBag.SelectedDiclist = model.TitleID;
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Update(ViewtQuestion model)
        {
            try
            {
                tQuestionServiceClient client = new tQuestionServiceClient();
                ViewtQuestion entity = client.GetById(model.ID);
                entity.TitleID = model.TitleID;
                entity.Hot = model.Hot;
                entity.QuestionContent = model.QuestionContent;
                entity.ReplyContent = model.ReplyContent;
                entity.ReplyUid = model.ReplyUid;
                //回答时间在这里作为修改时间
                entity.ReplyTime = DateTime.Now;
                client.Edit(entity);
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        public ActionResult Reply(int id)
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            SelectList Diclist = new SelectList(DicItemlist, "ID", "ItemName");
            ViewBag.Diclist = Diclist;
            tQuestionServiceClient client = new tQuestionServiceClient();
            ViewtQuestion model = client.GetById(id);
            ViewBag.Model = model;
            ViewBag.SelectedDicItemName = DicItemlist.First(p => p.ID == model.TitleID).ItemName;
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Reply(ViewtQuestion model)
        {
            try
            {
                tQuestionServiceClient client = new tQuestionServiceClient();
                ViewtQuestion entity = client.GetById(model.ID);
                entity.ReplyContent = model.ReplyContent;
                entity.ReplyUid = model.ReplyUid;
                entity.ReplyTime = DateTime.Now;
                entity.State = 2;
                client.Edit(entity);
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
    }
}
