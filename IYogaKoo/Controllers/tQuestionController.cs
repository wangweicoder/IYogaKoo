using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace IYogaKoo.Controllers
{
    public class tQuestionController : Controller
    {
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();
        YogisModelsServiceClient client;
        YogaUserServiceClient clientUser;
        YogaUserDetailServiceClient clientDetail;


        method method;
        public tQuestionController()
        {
            ViewBag.user = user;
            client = new YogisModelsServiceClient();
            clientUser = new YogaUserServiceClient();

            clientDetail = new YogaUserDetailServiceClient();

            method = new method();

            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = clientDetail.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                    ViewBag.level = temp.Ulevel;
            }
            else
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = client.GetYogisModelsById(user.Uid);
                if (vyogism != null)
                    ViewBag.level = vyogism.YogisLevel;
            }
            #endregion
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

        public ActionResult FAQDetail(int id)
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            ViewBag.Diclist = DicItemlist;
            tQuestionServiceClient client = new tQuestionServiceClient();
            var model = client.GetById(id);
            string whereStr = "";
            int count = 0;
            whereStr += "IsFAQ!" + true + ",";
            whereStr += "IsDelete!" + false + ",";
            whereStr += "Hot!" + true + ",";
            List<ViewtQuestion> hotList = client.GetList(whereStr, 1, 6, out count);
            ViewBag.HotList = hotList;
            return View(model);
        }

        public ActionResult HelpSearch(string questionContent, int page = 1)
        {
            ViewBag.QuestionContent = questionContent;
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            ViewBag.Diclist = DicItemlist;
            string whereStr = "";
            int pagesize = 3;
            int count = 0;
            whereStr += "IsFAQ!" + true + ",";
            whereStr += "IsDelete!" + false + ",";
            if (!string.IsNullOrWhiteSpace(questionContent))
            {
                whereStr += "QuestionContent!" + questionContent + ",";
            }
            tQuestionServiceClient client = new tQuestionServiceClient();
            List<ViewtQuestion> list = client.GetList(whereStr, page, pagesize, out count);
            foreach (var item in list)
            {
                ViewtQuestion model = item;
                Regex regex = new Regex(@"<[^>]+>|</[^>]+>");
                string replyContent = regex.Replace(item.ReplyContent, "");
                model.ReplyContent = replyContent.Length > 300 ? replyContent.Substring(0, 297) + "..." : replyContent;
            }
            ViewBag.ViewtQuestionList = list;
            ViewBag.ViewtQuestionCount = count;
            whereStr = "";
            whereStr += "IsFAQ!" + true + ",";
            whereStr += "IsDelete!" + false + ",";
            whereStr += "Hot!" + true + ",";
            int count2 = 0;
            List<ViewtQuestion> hotList = client.GetList(whereStr, 1, 6, out count2);
            ViewBag.HotList = hotList;
            Webdiyer.WebControls.Mvc.PagedList<ViewtQuestion> pagelist = new Webdiyer.WebControls.Mvc.PagedList<ViewtQuestion>(list, page, pagesize, count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("HelpSearchList", pagelist);
            }
            return View(pagelist);
        }

        public ActionResult PersonalHelpCenters(int TitleID = -1, int page = 1)
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            ViewBag.Diclist = DicItemlist;
            ViewBag.SelectedDic = DicItemlist.OrderBy(p => p.ID).First().ItemName;
            string whereStr = "";
            int pagesize = 150;
            int count = 0;
            whereStr += "IsFAQ!" + true + ",";
            whereStr += "IsDelete!" + false + ",";
            //whereStr += "BeFrom!" + 2 + ",";
            if (TitleID != -1)
            {
                ViewBag.SelectedDic = DicItemlist.First(p => p.ID == TitleID).ItemName;
                whereStr += "TitleID!" + TitleID + ",";
            }
            else
                whereStr += "TitleID!" + DicItemlist.OrderBy(p => p.ID).First().ID + ",";
            tQuestionServiceClient client = new tQuestionServiceClient();
            List<ViewtQuestion> list = client.GetList(whereStr, page, pagesize, out count);
            ViewBag.ViewtQuestionList = list;
            ViewtQuestion model = new ViewtQuestion();
            model.UserName = user.NickName;
            model.ContactInformation = user.UEmail;
            return View(model);
        }

        public ActionResult Help()
        {
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2563);
            ViewBag.Diclist = DicItemlist;
            ViewBag.SelectedDic = DicItemlist.OrderBy(p => p.ID).First().ItemName;
            ViewtQuestion model = new ViewtQuestion();
            model.UserName = user.NickName;
            model.ContactInformation = user.UEmail;
            return View(model);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult AddFAQ(FormCollection collection)
        {
            //ViewtQuestion model
            ViewtQuestion model = new ViewtQuestion();
            tQuestionServiceClient client = new tQuestionServiceClient();
            model.Uid = user.Uid;
            model.QuestionTime = DateTime.Now;
            model.State = 1;
            model.BeFrom = 1;
            model.IsFAQ = false;
            model.UserName = collection["UserName"];
            model.ContactInformation = collection["ContactInformation"];
            model.QuestionContent = collection["QuestionContent"];
            model.TitleID = int.Parse(collection["iType"]);
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
    }
}
