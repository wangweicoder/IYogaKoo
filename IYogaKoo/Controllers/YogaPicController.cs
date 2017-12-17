using Commons.Helper;
using IYogaKoo.Client;
using IYogaKoo.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using zzfIBM.WebControls.Mvc;

namespace IYogaKoo.Controllers
{
    public class YogaPicController : Controller
    {
        //
        // GET: /YogaPic/
        ///获取用户信息cookie
        BasicInfo user = Commons.Helper.Login.GetCurrentUser();

        YogaPictureServiceClient client;
        YogisModelsServiceClient mclient;
        List<ViewYogaPicture> list;
        YogaUserDetailServiceClient yogauserclient;
        tWriteLogServiceClient wlogclient;
        method method;
        ClassServiceClient classclient;
        ClassReportServiceClient classRepotclient;
        public YogaPicController()
        {
            ViewBag.user = user;
            client = new YogaPictureServiceClient();
            mclient = new YogisModelsServiceClient();
            yogauserclient = new YogaUserDetailServiceClient();
            list = new List<ViewYogaPicture>();
            wlogclient = new tWriteLogServiceClient();
            classclient = new ClassServiceClient();
            method = new method();
            classRepotclient = new ClassReportServiceClient();
            #region 登录者的级别
            if (user.UserType == 0)
            {
                ViewYogaUserDetail temp = new ViewYogaUserDetail();
                temp = yogauserclient.GetYogaUserDetailById(user.Uid);
                if (temp != null)
                {
                    ViewBag.level = temp.Ulevel;
                    ViewBag.Gender = temp.Gender;
                }
            }
            else
            {
                ViewYogisModels vyogism = new ViewYogisModels();
                vyogism = mclient.GetYogisModelsById(user.Uid);
                if (vyogism != null)
                {
                    ViewBag.level = vyogism.YogisLevel;
                    ViewBag.Gender = vyogism.Gender;
                }
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
        public ActionResult Index(int page = 1)
        { 
            string fileInfo = GetAllFolder();
            string[] ids = fileInfo.Split(',');
            List<SelectListItem> selectlist = new List<SelectListItem>
            {
                 new SelectListItem { Text = "请选择相册", Value = "",Selected=true},
            };
            foreach (var i in ids)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    selectlist.Add(new SelectListItem { Text = i, Value = i, Selected = false });
                }
            }
            if (selectlist.Count() == 0)
            {
                selectlist.Add(new SelectListItem { Text = "请选择相册", Value = "", Selected = true });
            }
            ViewData["FName"] = selectlist;

            int count = 0;

            list = client.GetYogaPicturePageList(user.Uid, page, 10, out count);

            PagedList<ViewYogaPicture> pagelist = new PagedList<ViewYogaPicture>(list, page, 10, count);

            return View(pagelist);

        }

        public ActionResult WebIndex(int page = 1)
        {
            list = client.GetUidList(user.Uid);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            ViewBag.jsonString = serializer.Serialize(list);

            return View(list);

        }
        //
        // GET: /YogaPicture/Details/5

        public ActionResult Details(int id)
        {
            list = client.GetUidList(id);

            return View(list);
        }

        // GET: /YogaPic/Create

        public ActionResult Create(string pictitle)
        {
            ViewBag.pictitle = pictitle;
            string fileInfo = GetAllFolder();
            string[] ids = fileInfo.Split(',');
            List<SelectListItem> list = new List<SelectListItem>
            {
                // new SelectListItem { Text = "请选择", Value = "",Selected=true},
            };
            foreach (var i in ids)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    list.Add(new SelectListItem { Text = i, Value = i, Selected = false });
                }
            }
            if (list.Count() == 0)
            {
                list.Add(new SelectListItem { Text = "请选择", Value = "", Selected = true });
            }
            ViewData["PictureName"] = list;
            return View();
        }

        //
        // POST: /YogaPic/Create

        [HttpPost]
        public JsonResult Create(ViewYogaPicture model)
        {
            try
            {
                // TODO: Add insert logic here
                if (!string.IsNullOrEmpty(Request.Form["Diploma"]))
                {
                    //相册
                    string[] strPic = Request.Form["Diploma"].ToString().Split(';');
                    string[] strpicContent = Request.Form["PictureContent"].ToString().TrimEnd('|').Split('|');

                    ViewYogaPicture picModel = new ViewYogaPicture();
                    for (int i = 0; i < strPic.Length - 1; i++)
                    {
                        #region
                        if (!string.IsNullOrEmpty(strPic[i]))
                        {
                            picModel.PictureOriginal = strPic[i];

                            picModel.Uid = user.Uid;

                            picModel.PictureType = 2;
                            try
                            {
                                picModel.PictureContent = strpicContent[i];
                            }
                            catch
                            {
                                picModel.PictureContent = "";
                            }
                            picModel.CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            picModel.CreateUser = user.Uid;//登录用户ID
                            picModel.PictureName =model.PictureName;
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
                            picModel.HitNum = 0;
                            picModel.iAudio = 0;
                            using (YogaPictureServiceClient clientpic = new YogaPictureServiceClient())
                            {
                                clientpic.Add(picModel);
                            }
                        }
                        #endregion
                    }

                }
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        //
        // GET: /YogaPic/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /YogaPic/Edit/5

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

       
        /// <summary>
        /// 查看相册
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MyPics(int imgtypeid=-1)
        {
            ViewBag.imgtypeid = imgtypeid;
            PicListGroup entity = new PicListGroup();
            
            List<PicListGroup> list = new List<PicListGroup>();
                    
            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + user.Uid);
            string uploadPathNext = string.Empty;
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            DirectoryInfo[] dirlist = dir.GetDirectories();
            
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                entity = new PicListGroup();
                entity.uid = user.Uid.ToString();
                entity.pictitle = subdir.Name;
                
                List<ViewYogaPicture> piclist = client.GetBackUidList(user.Uid, subdir.Name);
                entity.picnum = piclist.Count();
                if (piclist.Count() > 0)
                {
                    entity.picsrc = piclist[0].PictureOriginal;
                }
                else {
                    entity.picsrc = "../Content/Front/images/filebackimg.png";
                }
                //uploadPathNext = uploadPath + "\\" + subdir.Name;
                // DirectoryInfo dirNext = new DirectoryInfo(uploadPathNext); 
                //FileInfo[] files = dirNext.GetFiles("*.*");
                //entity.picnum = files.Count();
                //if (files.Count() > 0)
                //{
                //    entity.picsrc = "/Files/PirtureType/2/" + user.Uid + "/" + subdir.Name + "/" + files[0].ToString();
                //}
                list.Add(entity);
            }
            //所有图片:日志图片/习练笔记图片app/活动图片 -->根据PictureName="日志相册" 获取
            List<ViewYogaPicture> piclistAll = client.GetBackUidList(user.Uid);
            ViewBag.piclistAll = piclistAll;
             
            return View(list.Take(4).ToList());
        }

        /// <summary>
        /// 更多（全部相册）文件夹
        /// </summary>
        /// <returns></returns>
        public ActionResult PhotoFileInfo()
        {
            PicListGroup entity = new PicListGroup();

            List<PicListGroup> list = new List<PicListGroup>();

            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + user.Uid);
            string uploadPathNext = string.Empty;
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            DirectoryInfo[] dirlist = dir.GetDirectories();

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                entity = new PicListGroup();
                entity.uid = user.Uid.ToString();
                entity.pictitle = subdir.Name; 
                List<ViewYogaPicture> piclist = client.GetBackUidList(user.Uid, subdir.Name);
                entity.picnum = piclist.Count();
                if (piclist.Count() > 0)
                {
                    entity.picsrc = piclist[0].PictureOriginal;
                }
                else
                {
                    entity.picsrc = "../Content/Front/images/filebackimg.png";
                }
                // uploadPathNext = uploadPath + "\\" + subdir.Name;
                //DirectoryInfo dirNext = new DirectoryInfo(uploadPathNext);
                //FileInfo[] files = dirNext.GetFiles("*.*");
                //entity.picnum = files.Count();
                //if (files.Count() > 0)
                //{
                //    entity.picsrc = "/Files/PirtureType/2/" + user.Uid + "/" + subdir.Name + "/" + files[0].ToString();
                //}
                list.Add(entity);
            }
            //所有图片:日志图片/习练笔记图片app/活动图片 -->根据PictureName="日志相册" 获取
            List<ViewYogaPicture> piclistAll = client.GetBackUidList(user.Uid);
            ViewBag.piclistAll = piclistAll;
             
            return View(list);
        }
        /// <summary>
        /// 查看相片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MyPicsDetails(string id)
        {
            ViewBag.pictitle = id;
            PicListGroup entity = new PicListGroup();

            List<PicListGroup> listGroup = new List<PicListGroup>();

            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + user.Uid);
            string uploadPathNext = string.Empty;
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            DirectoryInfo[] dirlist = dir.GetDirectories();

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                entity = new PicListGroup();
                entity.uid = user.Uid.ToString();
                entity.pictitle = subdir.Name;
                List<ViewYogaPicture> piclist = client.GetBackUidList(user.Uid, subdir.Name);
                entity.picnum = piclist.Count();
                if (piclist.Count() > 0)
                {
                    entity.picsrc = piclist[0].PictureOriginal;
                }
                else
                {
                    entity.picsrc = "../Content/Front/images/filebackimg.png";
                }
                //uploadPathNext = uploadPath + "\\" + subdir.Name;
                //DirectoryInfo dirNext = new DirectoryInfo(uploadPathNext);
                //FileInfo[] files = dirNext.GetFiles("*.*");
                //entity.picnum = files.Count();
                //if (files.Count() > 0)
                //{
                //    entity.picsrc = "/Files/PirtureType/2/" + user.Uid + "/" + subdir.Name + "/" + files[0].ToString();
                //}
                listGroup.Add(entity);
            }
            ViewBag.listGroup = listGroup.Take(4).ToList();
            //所有图片:日志图片/习练笔记图片app/活动图片 -->根据PictureName="日志相册" 获取
            List<ViewYogaPicture> piclistAll = client.GetBackUidList(user.Uid);
            ViewBag.piclistAll = piclistAll;
              
            list = client.GetBackUidList(user.Uid, id);
            return View(list);
        }
        //
        // GET: /YogaPic/Delete/5

        public ActionResult Delete(int id)
        {
            //client.Delete(id.ToString());

            return RedirectToAction("Index");

        }
        //
        // POST: /YogaPic/Delete/5

        [HttpPost]
        public JsonResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                ViewYogaPicture model = client.GetById(id);
                string PictureOriginal = model.PictureOriginal;
                client.Delete(id.ToString());
                //删除文件夹中的图片
                PubClass.FileDel(Server.MapPath(PictureOriginal));
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        /// <summary>
        /// 编辑文件夹名
        /// </summary>
        /// <returns></returns>
        public JsonResult EditFileInfo(string OldFileName,string NewFileName)
        {
            try
            {
                 
                string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + user.Uid);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);

                }
                if (Directory.Exists(uploadPath + "/" + OldFileName))
                {
                    Directory.Move(uploadPath + "/" + OldFileName, uploadPath + "/" + NewFileName);
                   list= client.GetListWhere(user.Uid, OldFileName);
                   foreach (ViewYogaPicture i in list)
                   {
                       i.PictureName = NewFileName;
                       i.PictureOriginal = i.PictureOriginal.Replace(OldFileName, NewFileName);
                       client.Update(i);
                   }
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 1 });
                }

            }
            catch (Exception)
            {
                return Json(new { code = 1 });
            }
             
        }

        /// <summary>
        /// 递归删除文件夹及文件夹中的图片
        /// </summary>
        /// <param name="dirInfoName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteFileImg(string dirInfoName)
        {
            try
            {

                string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + user.Uid);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);

                }
                if (Directory.Exists(uploadPath + "/" + dirInfoName))
                {
                    Directory.Delete(uploadPath + "/" + dirInfoName, true);
                    list = client.GetListWhere(user.Uid, dirInfoName);
                    foreach (ViewYogaPicture i in list)
                    { 
                        client.Delete(i.Pid.ToString());
                    }
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 1 });
                }

            }
            catch (Exception)
            {
                return Json(new { code = 1 });
            }
        }
         
        [HttpPost]
        public JsonResult Info(FormCollection collection)
        {
            try
            {
                string txtpicName = collection["txtpicName"];

                string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + user.Uid);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);

                }
                if (!Directory.Exists(uploadPath + txtpicName))
                {
                    Directory.CreateDirectory(uploadPath + "/" + txtpicName);
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 1 });
                }

            }
            catch (Exception)
            {
                return Json(new { code = 1 });
            }

        }

        /// <summary> 
        /// 获取指定目录下的所有文件夹名 
        /// </summary> 
        /// <param name="path">目录路径</param> 
        /// <returns>string，返回所有文件夹名字</returns> 
        public string GetAllFolder()
        {
            string folder_Names = "";

            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + user.Uid);
            string uploadPathNext = string.Empty;
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);

            }
            DirectoryInfo dir = new DirectoryInfo(uploadPath);
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                folder_Names += subdir.Name + ",";

                //uploadPathNext = uploadPath + "\\" + subdir.Name;
                //DirectoryInfo dirNext = new DirectoryInfo(uploadPathNext);
                //FileInfo[] files = dirNext.GetFiles("*.*");

                //if (files.Count() > 0)
                //{
                //    foreach (var subdirNext in files)
                //    {
                //        //判断文件夹中是否存在图片
                //        // subdirNext.Name
                //        list = client.GetListWhere(user.Uid, subdirNext.Name);
                //        if (list.Count == 0)
                //        {
                //            PubClass.FileDel(uploadPathNext + "\\" + subdirNext.ToString());
                //        }
                //    }
                //}
            }
            return folder_Names.TrimEnd(',');
        }
    }
}
