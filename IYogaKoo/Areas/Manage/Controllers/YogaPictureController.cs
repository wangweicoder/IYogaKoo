using IYogaKoo.Client;
using IYogaKoo.Controllers;
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
    /// <summary>
    /// 图片管理
    /// </summary>
    public class YogaPictureController : Controller
    {
        //
        // GET: /Manage/YogaPicture/
        YogaPictureServiceClient client;
        List<ViewYogaPicture> list;
        YogaUserDetailServiceClient userDetclient; 
        YogaUserServiceClient userclient;
        YogisModelsServiceClient modelclient;
        public YogaPictureController()
        {
            client = new YogaPictureServiceClient();
            list = new List<ViewYogaPicture>();
            userDetclient = new YogaUserDetailServiceClient();
            userclient = new YogaUserServiceClient();
            modelclient = new YogisModelsServiceClient();
           
        } 
        public ActionResult IndexSearch(int page=1)
        { 
            int count = 0;
            
           // list=client.GetYogaPicturePageList(page, 10, out count);
           var  list2 = client.GetYogaPicturePageList(0).Where(a=>a.PictureType==2);

           var finq = (from l in list2

                        group l by new
                        { 
                            PictureName = l.PictureName,
                            Uid = l.Uid,
                            iAudio = l.iAudio,
                            CreateTime = l.CreateTime
                        } into grouped

                        orderby grouped.Key.CreateTime descending

                        select new ViewYogaPicture
                        {
                            PictureName = grouped.Key.PictureName,
                            Uid = grouped.Key.Uid,
                            iAudio = grouped.Key.iAudio,
                            CreateTime = grouped.Key.CreateTime
                        });
            count = finq.Count();
            var finqlist=finq.Skip((page - 1) * 10).Take(10).ToList();
            PagedList<ViewYogaPicture> pagelist = new PagedList<ViewYogaPicture>(finqlist, page, 10, count);
            List<ViewYogaPictureGroup> listGroup = new List<ViewYogaPictureGroup>();

            foreach (var item in finqlist)
            {
                ViewYogaPictureGroup model = new ViewYogaPictureGroup();

                model.entity = item;

                using (YogisModelsServiceClient uclient = new YogisModelsServiceClient())
                {
                    ViewYogisModels mm = uclient.GetYogisModelsById(item.Uid);
                    if (mm != null)
                    {
                        model.RealName = mm.RealName;
                        if (string.IsNullOrEmpty(mm.RealName))
                        {
                            ViewYogaUserDetail mmm = userDetclient.GetYogaUserDetailById(item.Uid);
                            if (mmm != null)
                                model.RealName = mmm.RealName_cn;
                        }
                    }
                    else
                    {
                        ViewYogaUserDetail mmm = userDetclient.GetYogaUserDetailById(item.Uid); 
                        if(mmm!=null)
                            model.RealName = mmm.RealName_cn; 
                        if (mmm!=null)
                        {
                            model.RealName = mmm.RealName_cn;
                        } 
                    }
                }

                listGroup.Add(model);
            }

            ViewBag.listGroup = listGroup;

            return View(pagelist);
             
        }

        public PartialViewResult Index(string Uid,DateTime? createTime, int page = 1)
        {
            int count = 0;

            // list=client.GetYogaPicturePageList(page, 10, out count);
            list = client.GetYogaPicturePageList(0);

            var finq = (from l in list

                        group l by new
                        {
                            PictureType = l.PictureType,
                            Uid = l.Uid,
                            iAudio = l.iAudio,
                            CreateTime = l.CreateTime
                        } into grouped

                        orderby grouped.Key.CreateTime descending

                        select new ViewYogaPicture
                        {
                            PictureType = grouped.Key.PictureType,
                            Uid = grouped.Key.Uid,
                            iAudio = grouped.Key.iAudio,
                            CreateTime = grouped.Key.CreateTime
                        });
            if (!string.IsNullOrEmpty(Uid))
            {
                ViewYogaUserDetail model = userDetclient.GetByRealName(Uid);
                if (model != null)
                {
                    finq = finq.Where(x => x.Uid == model.UID);
                }
                else
                {
                    ViewYogisModels models = modelclient.GetByRealName(Uid);
                    if (models != null)
                    {
                        finq = finq.Where(x => x.Uid == models.UID);
                    }
                }
            } 
            if (createTime != null)
            {
                finq = finq.Where(x => x.CreateTime>= createTime.Value);
            }
            count = finq.Count();
            var finqlist = finq.OrderBy(x=>x.CreateTime).Skip((page - 1) * 10).Take(10).ToList();
            PagedList<ViewYogaPicture> pagelist = new PagedList<ViewYogaPicture>(finqlist, page, 10, count);
            List<ViewYogaPictureGroup> listGroup = new List<ViewYogaPictureGroup>();

            foreach (var item in finqlist)
            {
                ViewYogaPictureGroup model = new ViewYogaPictureGroup();

                model.entity = item;

                using (YogisModelsServiceClient uclient = new YogisModelsServiceClient())
                {
                    ViewYogisModels mm = uclient.GetYogisModelsById(item.Uid);
                    if (mm != null)
                    {
                        model.RealName = mm.RealName;
                        if (string.IsNullOrEmpty(mm.RealName))
                        {
                            ViewYogaUserDetail mmm = userDetclient.GetYogaUserDetailById(item.Uid);
                            if (mmm != null)
                                model.RealName = mmm.RealName_cn;
                        }
                    }
                    else
                    {
                        ViewYogaUserDetail mmm = userDetclient.GetYogaUserDetailById(item.Uid);
                        if (mmm != null)
                            model.RealName = mmm.RealName_cn;
                        if (mmm != null)
                        {
                            model.RealName = mmm.RealName_cn;
                        }
                    }
                }

                listGroup.Add(model);
            }

            ViewBag.listGroup = listGroup;

            return PartialView("Index", pagelist);

        }
        //
        // GET: /Manage/YogaPicture/Details/5
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id,DateTime? CreateTime, string FName)
        {

            ViewBag.Uid = id;
            ViewYogaPicture picInfo = new ViewYogaPicture();
            if (CreateTime != null)
            {
                picInfo = client.GetYogaPictureByCreateTime(id, CreateTime.Value);
            }
            else if (!string.IsNullOrEmpty(FName))
            {
                picInfo = client.GetYogaPictureById(id, FName);
            }
            else
            {
                picInfo = client.GetYogaPictureById(id);
            }
            if (picInfo == null)
            {
                return Content("<script>alert('已经全部删除！');window.location='/YogaPicture/IndexSearch';</script>");
            }
            //获取相册名称（文件夹）
            string fileInfo = GetAllFolder(id);
            string[] ids = fileInfo.Split(',');
            if (ids.Count() > 0)
            {
                List<SelectListItem> selectlist = new List<SelectListItem> { };
                foreach (var i in ids)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        selectlist.Add(new SelectListItem { Text = i, Value = i, Selected = false });
                    }
                }
                
                ViewData["FName"] = selectlist;
            }
            //
            #region
            
            ViewBag.picInfo = picInfo;
            ViewYogisModels model = modelclient.GetYogisModelsById(id);
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.RealName))
                {
                    ViewBag.Name = model.RealName;
                }
                else
                {
                    ViewYogaUserDetail detEntity = userDetclient.GetYogaUserDetailById(id);
                    if (detEntity != null)
                    {
                        if (!string.IsNullOrEmpty(detEntity.RealName_cn))
                        {
                            ViewBag.Name = detEntity.RealName_cn;
                        }
                    }
                    else
                    {
                        ViewYogaUser userEntity = userclient.GetYogaUserById(id);

                        if (userEntity != null)
                        {
                            if (!string.IsNullOrEmpty(userEntity.NickName))
                            {
                                ViewBag.Name = userEntity.NickName;
                            }
                        }
                    }
                }
            }
            else {
                ViewYogaUserDetail detEntity = userDetclient.GetYogaUserDetailById(id);
                if (detEntity != null)
                {
                    if (!string.IsNullOrEmpty(detEntity.RealName_cn))
                    {
                        ViewBag.Name = detEntity.RealName_cn;
                    }
                }
                else
                {
                    ViewYogaUser userEntity = userclient.GetYogaUserById(id);

                    if (userEntity != null)
                    {
                        if (!string.IsNullOrEmpty(userEntity.NickName))
                        {
                            ViewBag.Name = userEntity.NickName;
                        }
                    }
                }
            }
            #endregion
            if (!string.IsNullOrEmpty(FName))
            {
                list = client.GetBackUidList(id, FName);
            }
            else if (CreateTime != null)
            {
                ViewBag.CreateTime = CreateTime;
                list = client.GetBackUidList(id, CreateTime.Value);
            }
            else
            {
                list = client.GetBackUidList(id);
            }
            ViewBag.list = list;
            return View();
        }

        /// <summary> 
        /// 获取指定目录下的所有文件夹名 
        /// </summary> 
        /// <param name="path">目录路径</param> 
        /// <returns>string，返回所有文件夹名字</returns> 
        public string GetAllFolder(int id)
        {
            string folder_Names = "";

            string uploadPath = Server.MapPath("~/Files/PirtureType/2/" + id);
            //string uploadPathNext = string.Empty;
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

        //
        // GET: /Manage/YogaPicture/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage/YogaPicture/Create

        [HttpPost]
        public ActionResult Create(ViewYogaPicture model)
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
        // GET: /Manage/YogaPicture/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Manage/YogaPicture/Edit/5

        [HttpPost]
        public ActionResult Edit(ViewYogaPicture model)
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
        // GET: /Manage/YogaPicture/Delete/5
         [HttpPost]
        public JsonResult Deleteids(string ids)
        {
            ViewYogaPicture model = new ViewYogaPicture();
            string[] ilist = ids.TrimEnd(',').Split(',');
            foreach (var i in ilist)
            {
                model = client.GetById(Convert.ToInt32(i));
                PubClass.FileDel(Server.MapPath(model.PictureOriginal));

                client.Delete(i.ToString());
            }
            return Json(new { code=0});
        }
        public ActionResult Delete(int id, DateTime? CreateTime)
        {
            ViewYogaPicture model = new ViewYogaPicture();
            if (CreateTime != null)
            {
                list = client.GetBackUidList(id,CreateTime.Value);
            }
            else {
                list = client.GetBackUidList(id);
            }
            foreach (var item in list)
            {
                model = item;
                //删除文件夹中的图片
                PubClass.FileDel(Server.MapPath(model.PictureOriginal));

                client.Delete(model.Pid.ToString());
                
            }
            return RedirectToAction("IndexSearch");
        }

        //
        // POST: /Manage/YogaPicture/Delete/5

        [HttpPost]
        public JsonResult Delete(int id, DateTime? CreateTime, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                if (CreateTime != null)
                {
                    list = client.GetBackUidList(id, CreateTime.Value);
                    foreach (var item in list)
                    { 
                        //删除文件夹中的图片
                        PubClass.FileDel(Server.MapPath(item.PictureOriginal));

                        client.Delete(item.Pid.ToString());

                    }
                }
                else
                {
                    client.Delete(id.ToString()); 
                }
                //删除文件夹中的图片
                PubClass.FileDel(Server.MapPath(collection["PictureOriginal"]));

                return Json(new { code=0});
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AudioState()
        {
            try
            {
                // TODO: Add delete logic here
                ViewYogaPicture model = new ViewYogaPicture();

                int Uid = Convert.ToInt32(Request.Form["Uid"]);
                int iAudio = Convert.ToInt32(Request.Form["iAudio"]);
                string FName = string.Empty;
                DateTime dtTime = new DateTime();
                if (Request.Form["FName"] != null)
                {
                    FName = Request.Form["FName"].ToString();
                    if (Request.Form["CreateTime"] != null)
                    { 
                        dtTime = Convert.ToDateTime(Request.Form["CreateTime"]);
                        //list = client.GetBackUidList(Uid, FName,dtTime);
                        int count = 0;
                        list =client.GetBackPageList(Uid.ToString(), dtTime, out count);
                    }
                    else
                    {
                      //  FName = Request.Form["FName"].ToString();
                        list = client.GetBackUidList(Uid, FName);
                    }
                }
                else 
                {
                    list = client.GetBackUidList(Uid);
                }
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        model = item;
                        model.iAudio = iAudio;
                        client.Update(model);
                    }
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 1 });
                }

            }
            catch
            {
                return Json(new { code = 1 });
            }

        }

        /// <summary>
        /// 修改描述
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PictureContentEdit(FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int Pid = Convert.ToInt32(collection["Pid"]);
                string strPictureContent = collection["PictureContent"];

                ViewYogaPicture model = client.GetById(Pid);
                if (model != null)
                {
                    model.PictureContent = strPictureContent;
                    client.Update(model);
                    return Json(new { code = 0 });
                }
                else {
                    return Json(new { code = 1 });
                }
                
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }
    }
}
