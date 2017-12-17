using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Commons.Helper;
using System.Text.RegularExpressions;

namespace IYogaKoo.Areas.Manage.Controllers
{
    public class WriteLogController : Controller
    {
        //
        // GET: /Manage/WriteLog/
        tWriteLogServiceClient client;
        YogaUserServiceClient userClient;
        List<ViewtWriteLog> list;
        ViewtWriteLog model; 
        YogaUserDetailServiceClient userDetclient; 
        YogisModelsServiceClient modelclient;
        method method;
        tLearingServiceClient learclient;
        YogaPictureServiceClient picclient;
        public WriteLogController()
        {
            model = new ViewtWriteLog();
            client = new tWriteLogServiceClient();
            list = new List<ViewtWriteLog>();
            userClient = new YogaUserServiceClient();
            userDetclient = new YogaUserDetailServiceClient();
            modelclient = new YogisModelsServiceClient();
            method = new method();
            learclient=new tLearingServiceClient ();
            picclient = new YogaPictureServiceClient();
        }
        public ActionResult IndexSearch(int page = 1)
        {
            int count = 0;

            list = client.GettWriteLogPageList(page, 10, out count);

            PagedList<ViewtWriteLog> pagelist = new PagedList<ViewtWriteLog>(list, page, 10, count);

            List<ViewtWriteLogGroup> listGroup = new List<ViewtWriteLogGroup>();
            foreach (var item in list)
            {
                ViewtWriteLogGroup model = new ViewtWriteLogGroup();
                model.entity = item;
                ViewYogaUser userEntity = userClient.GetYogaUserById(item.Uid.Value);
                if (userEntity != null)
                    model.UserName = userEntity.NickName;

                listGroup.Add(model);
            }
            ViewBag.listGroup = listGroup;

            return View(pagelist);
        }

        public PartialViewResult Index(string Uid, string sTitle, DateTime? createTime, int page = 1)
        {
            int count = 0;
            
            int iUid = 0;//默认
            if (!string.IsNullOrEmpty(Uid))
            {
                ViewYogaUser model = userClient.ExistNickName(Uid);
                if (model != null)
                {
                    iUid = model.Uid;
                } 
            }
             
            list = client.GettWriteLogPageList(iUid,sTitle,createTime,page, 10, out count);
            PagedList<ViewtWriteLog> pagelist = new PagedList<ViewtWriteLog>(list, page, 10, count);

            List<ViewtWriteLogGroup> listGroup = new List<ViewtWriteLogGroup>();
            foreach (var item in list)
            {
                ViewtWriteLogGroup model = new ViewtWriteLogGroup();
                model.entity = item;
                ViewYogaUser userEntity = userClient.GetYogaUserById(item.Uid.Value);
                if (userEntity != null)
                {
                    model.UserName = userEntity.NickName;
                }
                else model.UserName = "";
                listGroup.Add(model);
            }
            ViewBag.listGroup = listGroup;

            return PartialView("Index", pagelist);
        }
         
        //
        // GET: /Manage/WriteLog/Details/5

        public ActionResult Details(int id)
        {
            ViewtWriteLog model = client.GetById(id);
            ViewYogaUser userEntity = userClient.GetYogaUserById(model.Uid.Value);
            if(userEntity!=null)
            {
                ViewBag.NickName = userEntity.NickName;
            }
            return View(model);
        }

        //
        // GET: /Manage/WriteLog/Create

        public ActionResult Create(int? uid)
        {
            if (uid != null)
            {
                ViewBag.Uid = uid.Value;
            }

            return View();
        }

        //
        // POST: /Manage/WriteLog/Create

        [HttpPost,ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                model.CreateDate = DateTime.Now;
                model.ifShow = true;
                model.iReadNums = 0;
                model.ValueType = 0;
                model.sContent = collection["sContent"].ToString();
                model.sTitle = collection["sTitle"].ToString();
                model.Uid =Convert.ToInt32(collection["Uid"]);
                client.Add(model);
                //start 把sContent中图片添加到相册YogaPicture，类型：3

                Regex rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
                var m = rg.Match(model.sContent);
                while (m.Success)
                {
                    ViewYogaPicture picModel = new ViewYogaPicture();
                    picModel.PictureOriginal = m.Groups[1].Value;//这里就是图片路径     
                    picModel.PictureType = 3;
                    picModel.CreateTime = DateTime.Now;
                    picModel.PictureName = "日志相册";
                    picModel.Uid = 100316;//管理员
                    picModel.CreateUser = 100316;
                    picModel.PictureContent = "日志相册";
                    picModel.HitNum = 0;
                    picModel.iAudio = 1;
                     
                    picModel.PictureSmall = "";
                    picModel.AlbumId = 0;
                    picModel.EvaluateId = 0;
                    picModel.Comid = 0;
                    picModel.PictureLarge = "";
                    picModel.PictureMiddle = "";
                    picModel.PircureSize = "";
                    picModel.CommentCount = 0;
                    picModel.LikeCount = 0;
                    picModel.NotLikeCount = 0;
                    picModel.CommentLimite = 0;
                    picModel.LastChangeTime = DateTime.Now;
                    picclient.Add(picModel);
                    m = m.NextMatch();
                }

                //end
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/WriteLog/Edit/5

        public ActionResult Edit(int id)
        {
            model = client.GetById(id);
            
            return View(model);
        }

        //
        // POST: /Manage/WriteLog/Edit/5

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                model = client.GetById(id);
                model.ifShow = Convert.ToBoolean(collection["ifShow"]);
                model.sContent = collection["sContent"].ToString();
                model.sTitle = collection["sTitle"].ToString();
                model.Uid = Convert.ToInt32(collection["Uid"]);
                client.Update(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/WriteLog/Delete/5

        public ActionResult Delete(int id)
        {
            client.Delete(id.ToString());
            return RedirectToAction("Index");
        }

        //
        // POST: /Manage/WriteLog/Delete/5

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

        /// <summary>
        /// 推送：向tLearing表添加文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="iType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddtLearing(int id,int iType)
        {
            try
            {
                model = client.GetById(id);
                ViewtLearing tLearnentity = learclient.ExistsTitle(model.Uid.ToString(), model.sTitle);
                if (tLearnentity == null)
                {
                    ViewtLearing entity = new ViewtLearing();
                    entity.Uid = model.Uid.ToString();
                    entity.sTitle = model.sTitle;
                    entity.sContent = model.sContent;
                    entity.sPic = method.getImgUrl(model.sContent)[0].ToString();
                    entity.iType = iType;
                    entity.ifexamine = true;
                    entity.UrlType = 0;
                    entity.CreateDate = DateTime.Now;
                    entity.iReadNums = 0;
                    entity.iWritelogNums = 0;
                    entity.iZanNums = 0;
                    
                    learclient.Add(entity);
                     
                    return Json(new { code = 0 });
                } 
                return Json(new { code = 2 });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1 });
            }
        }
    }
}
