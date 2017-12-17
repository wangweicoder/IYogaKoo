using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.Entity;
using IYogaKoo.ViewModel;
using Webdiyer.WebControls.Mvc;
namespace IYogaKoo.Areas.Manage.Controllers
{ 
    public class tLearingController : BaseController
    {
        //社区

        //
        // GET: /Manage/tLearing/
        tLearingServiceClient client;
        YogaUserServiceClient clientUser;
        List<ViewtLearing> list;
        ViewtLearing model;
        method method;
        tMessageServiceClient msgclient;
        public tLearingController()
        {
            client = new tLearingServiceClient();
            clientUser = new YogaUserServiceClient();
            list = new List<ViewtLearing>();
            model = new ViewtLearing();
            method = new method();
            msgclient = new tMessageServiceClient();
        }

        public ActionResult IndexSearch(string NickName,string sTitle,DateTime? CreateDate,int? iType, int page = 1)
        {
            int count = 0;
            int pagesize = 10;
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            string iUid = "";
            if (!string.IsNullOrEmpty(NickName))
            {
                ViewYogaUser model = clientUser.ExistNickName(NickName);
                if (model != null)
                {
                    iUid = model.Uid.ToString();
                }
            }
            List<ViewtLearing> list = client.GetPageList(iUid,sTitle,CreateDate,iType, page, pagesize, out count);
            for (var i = 0; i < list.Count(); i++)
            {
                list[i].iWritelogNums = msgclient.GettMessageUid(list[i].ID, 2).Count();
                list[i].NickName = method.GetNickName(Convert.ToInt32(list[i].Uid));
                foreach( var k in DicItemlist)
                {
                    if (list[i].iType == k.ID)
                    {
                        list[i].TypeValue = k.ItemName;
                    }
                } 
            }
            PagedList<ViewtLearing> pagelist = new PagedList<ViewtLearing>(list, page, pagesize,count);
 
            return View(pagelist);
        }

        public PartialViewResult Index(string NickName, string sTitle, DateTime? CreateDate, int? iType, int page = 1)
        {
            int count = 0;
            int pagesize = 10;
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            string iUid = "";
            if (!string.IsNullOrEmpty(NickName))
            {
                ViewYogaUser model = clientUser.ExistNickName(NickName);
                if (model != null)
                {
                    iUid = model.Uid.ToString();
                }
            }
            List<ViewtLearing> list = client.GetPageList(iUid, sTitle, CreateDate, iType, page, pagesize, out count);
            for (var i = 0; i < list.Count(); i++)
            {
                list[i].iWritelogNums = msgclient.GettMessageUid(list[i].ID, 2).Count();
                list[i].NickName = method.GetNickName(Convert.ToInt32(list[i].Uid));
                foreach (var k in DicItemlist)
                {
                    if (list[i].iType == k.ID)
                    {
                        list[i].TypeValue = k.ItemName;
                    }
                }
            }
            PagedList<ViewtLearing> pagelist = new PagedList<ViewtLearing>(list, page, pagesize,count);

            return PartialView("Index", pagelist);
        }

        #region 待审核推送日志

        public ActionResult ExamineSearch(string NickName, string sTitle, DateTime? CreateDate, int? iType, int page = 1)
        {
            int count = 0;
            int pagesize = 10;
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            string iUid = "";
            if (!string.IsNullOrEmpty(NickName))
            {
                ViewYogaUser model = clientUser.ExistNickName(NickName);
                if (model != null)
                {
                    iUid = model.Uid.ToString();
                }
            }
            List<ViewtLearing> list = client.GetExaminePageList(iUid, sTitle, CreateDate, iType, page, pagesize, out count);
            for (var i = 0; i < list.Count(); i++)
            {
                list[i].iWritelogNums = msgclient.GettMessageUid(list[i].ID, 2).Count();
                list[i].NickName = method.GetNickName(Convert.ToInt32(list[i].Uid));
                foreach (var k in DicItemlist)
                {
                    if (list[i].iType == k.ID)
                    {
                        list[i].TypeValue = k.ItemName;
                    }
                }
            }
            PagedList<ViewtLearing> pagelist = new PagedList<ViewtLearing>(list, page, pagesize, count);

            return View(pagelist);
        }

        public PartialViewResult ExamineIndex(string NickName, string sTitle, DateTime? CreateDate, int? iType, int page = 1)
        {
            int count = 0;
            int pagesize = 10;
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            string iUid = "";
            if (!string.IsNullOrEmpty(NickName))
            {
                ViewYogaUser model = clientUser.ExistNickName(NickName);
                if (model != null)
                {
                    iUid = model.Uid.ToString();
                }
            }
            List<ViewtLearing> list = client.GetExaminePageList(iUid, sTitle, CreateDate, iType, page, pagesize, out count);
            for (var i = 0; i < list.Count(); i++)
            {
                list[i].iWritelogNums = msgclient.GettMessageUid(list[i].ID, 2).Count();
                list[i].NickName =method. GetNickName(Convert.ToInt32(list[i].Uid));
                foreach (var k in DicItemlist)
                {
                    if (list[i].iType == k.ID)
                    {
                        list[i].TypeValue = k.ItemName;
                    }
                }
            }
            PagedList<ViewtLearing> pagelist = new PagedList<ViewtLearing>(list, page, pagesize, count);

            return PartialView("ExamineIndex", pagelist);
        }

        /// <summary>
        /// 审核按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExamineMethod(int id)
        { 
            try {
                model = client.GetById(id);
                model.ifexamine = true;
                client.Update(model);
                return Json(new { code=0});
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 }); 
            }
            
        }
        #endregion
         
        public ActionResult Create()
        { 
            ViewBag.Name = method.GetNickName(0);

            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Create(ViewtLearing model)
        {
            try
            {
                ViewtLearing entity = client.ExistsTitle("", model.sTitle);
                if (entity == null)
                {
                    model.Uid = "100316";
                    model.iWritelogNums = 0;
                    model.iZanNums = 0;
                    model.iReadNums = 0;
                    model.UrlType = 0;
                    model.ifexamine = true;
                    model.CreateDate = DateTime.Now;
                    client.Add(model);
                    return Json(new { code = 0 });
                }
                return Json(new { code = 2 });//code=2 已经存在该文章
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 });
            }

        }
        [HttpPost]
        public JsonResult Delete(int id, int iType)
        {
            try {

                client.Delete(id.ToString());

                if (iType == 1)
                {
                    return Json(new { code=0,iType=1});
                }
                else
                {
                    return Json(new { code = 0, iType = 0 });
                }

                
            }
            catch (Exception ex)
            { 
            
            }
            return Json(new { code = 1});
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="iType">1 审核列表；0 管理列表</param>
        /// <returns></returns>
        public ActionResult Details(int id,int iType)
        {
            ViewBag.Name = method.GetNickName(Convert.ToInt32(model.Uid));
            ViewBag.iType = iType;
            model = client.GetById(id);
            using (YogaDicItemServiceClient YogaDicItemServiceClient = new YogaDicItemServiceClient())
            {
                ViewBag.ItemName = YogaDicItemServiceClient.GetById(model.iType.Value).ItemName;
            }
            return View(model);　
        }

        public ActionResult Edit(int id, int iType)
        {
            ViewBag.iType = iType; 
            List<ViewYogaDicItem> DicItemlist = method.listDicItem(2158);
            ViewBag.Diclist = DicItemlist;
            model = client.GetById(id);

            ViewBag.Name = method.GetNickName(Convert.ToInt32(model.Uid));

            return View(model);
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult Edit(int id,  ViewtLearing Entity)
        {
            int webType = Convert.ToInt32(Request.Form["webType"]);
            Entity.ID = id;
            client.Update(Entity);

            if (webType == 1)
            { 
                return RedirectToAction("ExamineSearch");
            }
            else
            {
                return RedirectToAction("IndexSearch");
            }
        }
    }
}
